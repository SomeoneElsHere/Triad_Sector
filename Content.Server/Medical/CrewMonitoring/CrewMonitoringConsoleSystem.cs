using System.Linq;
using System.Threading;
using System.Xml;
using Content.Server._NF.CryoSleep;
using Content.Server.DeviceNetwork;
using Content.Server.DeviceNetwork.Systems;
using Content.Server.Medical.SuitSensors;
using Content.Server.PowerCell;
using Content.Server.Sound;
using Content.Shared.Damage;
using Content.Shared.DeviceNetwork;
using Content.Shared.DeviceNetwork.Events;
using Content.Shared.Medical.CrewMonitoring;
using Content.Shared.Medical.SuitSensor;
using Content.Shared.Mind.Components;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Pinpointer;
using Content.Shared.Sound.Components;
using Robust.Server.GameObjects;
using Robust.Shared.Audio.Systems;
using Robust.Shared.GameStates;
using Robust.Shared.Map;
using Robust.Shared.Utility;
namespace Content.Server.Medical.CrewMonitoring;

public sealed class CrewMonitoringConsoleSystem : EntitySystem
{
    [Dependency] private readonly PowerCellSystem _cell = default!;
    [Dependency] private readonly UserInterfaceSystem _uiSystem = default!;

    [Dependency] private readonly EmitSoundSystem _emitSound = default!;

    bool canPlaySound = true;

    List<EntityCoordinates> _coords = new List<EntityCoordinates>();

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<CrewMonitoringConsoleComponent, ComponentRemove>(OnRemove);
        SubscribeLocalEvent<CrewMonitoringConsoleComponent, DeviceNetworkPacketEvent>(OnPacketReceived);
        SubscribeLocalEvent<CrewMonitoringConsoleComponent, BoundUIOpenedEvent>(OnUIOpened);
        SubscribeLocalEvent<MobStateComponent, DamageChangedEvent>(OnDamageChanged);
    }

    private void OnRemove(EntityUid uid, CrewMonitoringConsoleComponent component, ComponentRemove args)
    {
        component.ConnectedSensors.Clear();
    }

    private void OnPacketReceived(EntityUid uid, CrewMonitoringConsoleComponent component, DeviceNetworkPacketEvent args)
    {
        var payload = args.Data;

        // Check command
        if (!payload.TryGetValue(DeviceNetworkConstants.Command, out string? command))
            return;

        if (command != DeviceNetworkConstants.CmdUpdatedState)
            return;

        if (!payload.TryGetValue(SuitSensorConstants.NET_STATUS_COLLECTION, out Dictionary<string, SuitSensorStatus>? sensorStatus))
            return;

        component.ConnectedSensors = sensorStatus;
        UpdateUserInterface(uid, component);

    }

    private void OnUIOpened(EntityUid uid, CrewMonitoringConsoleComponent component, BoundUIOpenedEvent args)
    {
        if (!_cell.TryUseActivatableCharge(uid))
            return;

        UpdateUserInterface(uid, component);
    }
    //The PR does the following.
    //Over 100 damage? Play a sound to add to overall dread. (explained in PR)
    //Over 128 crew consoles? Start skipping everything to preservere resourses.
    //Dont play that sound if it is in a 15 radius of another crew monitor to stop really loud sounds.
    //Dont play that sound if there is a cooldown to stop really loud sounds.
    //No sounds if we dont have a sensor; how would the crew console get it IC?

    private void SoundCooldown(object? obj) //cooldown for sound
    {
        canPlaySound = false;
        Thread.Sleep(1000);
        canPlaySound = true;

    }
    private void EnumarateCrewConsoles() //main loop, go through crew consoles and process them
    {
        int max = 0;
        foreach (EntityUid uid in EntityManager.AllEntityUids<CrewMonitoringConsoleComponent>()) //I dunno how to get all of the crew monitoring systems otherwise
        {
            if (max > 128) //if there are more than 128 crew monitoring consoles just ignore it
            {
                continue;
            }
            var transf = EntityManager.TransformQuery.Get(uid).Comp;
            var coord1 = transf.Coordinates;
            bool skip = false;
            foreach (EntityCoordinates nongrid in _coords)
            {
                if (coord1.TryDistance(EntityManager, nongrid, out float dist) && dist < 15) //if dist between 2 crew monitors is less than 15, skip playing it
                {

                    skip = true;
                    break;
                }
            }
            _coords.Add(coord1);
            if (skip)
            {
                continue;
            }

            var sound = EntityManager.GetComponent<EmitSoundOnSpawnComponent>(uid);

            if (sound.Sound == null) 
            {
                continue;
            }
            if (sound.Sound.Params.Volume != 15) //This sets the volume to -15 from -1000 to allow you to hear it onluy after it spawns.
            {
                var para = sound.Sound.Params;
                para.Volume = -15;  
                sound.Sound.Params = para;
                RemComp<EmitSoundOnSpawnComponent>(uid);
                AddComp<EmitSoundOnSpawnComponent>(uid, sound);
            }
            _emitSound.EmitSoundOverride(uid, sound); //do sound
            max++; //if there are more than 128 crew monitoring consoles just ignore it
        }
    }
    private bool FindSuit(TransformChildrenEnumerator enu) //checks if a child of the damaged person has a suit with sensors on them, if so we skip them because it doesnt really make sense for it to go off when you have no sensors.
    {
        while (enu.MoveNext(out EntityUid mightBeSuit))
        {
            if (EntityManager.TryGetComponent<SuitSensorComponent>(mightBeSuit, out SuitSensorComponent? _suit))
            {
                if (_suit.Mode != SuitSensorMode.SensorOff && !_suit.Jammed)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private void OnDamageChanged(EntityUid eu, MobStateComponent mobState, DamageChangedEvent args) //sound processing but I over engineered it
    {
        if (!EntityManager.TryGetComponent<PlayerJobComponent>(eu, out PlayerJobComponent? j)) //only player jobs count
        {
            return;
        }
        if (!canPlaySound) //cooldown
        {
            return;
        }
        var suitfinder = EntityManager.TransformQuery.Get(eu).Comp.ChildEnumerator;
        if (!FindSuit(suitfinder))// if the mob doesnt have a suit sensor on, skip it
        {
            return;
        }
        if (args.DamageDelta != null && args.DamageDelta.GetTotal().Value > 10000) //if the change in damage was over 100
        {
            EnumarateCrewConsoles(); //do main loop
            _coords.Clear();
            Thread t = new Thread(new ParameterizedThreadStart(SoundCooldown)); //start cooldown
            t.Start();
        }
    }
    //end changes
    private void UpdateUserInterface(EntityUid uid, CrewMonitoringConsoleComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        if (!_uiSystem.IsUiOpen(uid, CrewMonitoringUIKey.Key))
            return;

        // The grid must have a NavMapComponent to visualize the map in the UI
        var xform = Transform(uid);

        if (xform.GridUid != null)
            EnsureComp<NavMapComponent>(xform.GridUid.Value);

        // Update all sensors info
        var allSensors = component.ConnectedSensors.Values.ToList();
        _uiSystem.SetUiState(uid, CrewMonitoringUIKey.Key, new CrewMonitoringState(allSensors));

    }
}
