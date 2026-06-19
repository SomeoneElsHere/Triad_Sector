using Robust.Shared.Serialization;
using Robust.Shared.GameStates;
using Content.Shared._CorvaxNext.Silicons.Borgs;

namespace Content.Shared.Atmos.Components;

[NetworkedComponent]
[AutoGenerateComponentState]
[RegisterComponent]
public sealed partial class GasMinerComponent : Component
{
    /// <summary>
    ///     Operational state of the miner.
    /// </summary>
    [AutoNetworkedField]
    [ViewVariables(VVAccess.ReadOnly)]
    public GasMinerState MinerState = GasMinerState.Disabled;

    /// <summary>
    ///      If the number of moles in the external environment exceeds this number, no gas will be mined.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField]
    public float MaxExternalAmount = float.PositiveInfinity;

    /// <summary>
    ///      If the pressure (in kPA) of the external environment exceeds this number, no gas will be mined.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField]
    public float MaxExternalPressure = Atmospherics.GasMinerDefaultMaxExternalPressure;

    /// <summary>
    ///     Gas to spawn.
    /// </summary>
    private Gas _spawnGas;

    [ViewVariables(VVAccess.ReadWrite)] //cant apply enum properly
    [DataField(required: true)]
    public Gas SpawnGas
    {
        get
        {
           return _spawnGas; 
        } 
        set
        {
            _spawnGas = (int)value >= 128 ? (Gas)((((((int)value)- 128)*-1)+128)*-1) : value;
        }
    }

    /// <summary>
    ///     Temperature in Kelvin.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField]
    public float SpawnTemperature = Atmospherics.T20C;

    /// <summary>
    ///     Number of moles created per second when the miner is working.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField]
    public float SpawnAmount = Atmospherics.MolesCellStandard * 20f;
}

[Serializable, NetSerializable]
public enum GasMinerState : byte
{
    Disabled,
    Idle,
    Working,
}
