using System.Reflection;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Prototypes;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.Tips;
using Content.Shared.Weapons.Reflect;
using JetBrains.Annotations;
using Newtonsoft.Json.Serialization;
using Robust.Shared.Prototypes;
using Robust.Shared.Reflection;

namespace Content.Server.Atmos.EntitySystems;

[UsedImplicitly]
public sealed class CustomGasSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IReflectionManager _reflection = default!;
    public override void Initialize()
    {
        base.Initialize();
        SubscribeAllEvent<BoilingEvent>(OnBoiling);
        Type[] list = new Type[_prototypeManager.Count<ReagentPrototype>()];
        //fuck it we ball
        int i = 0;
        foreach (ReagentPrototype reagent in _prototypeManager.GetInstances<ReagentPrototype>().Values)
        {
            GasPrototype gas = new GasPrototype
            {
                Name = reagent.LocalizedName,
                MolarMass = 3*reagent.SpecificHeat, //https://en.wikipedia.org/wiki/Dulong%E2%80%93Petit_law
                isChemicalGas = true;
            };
            SetPropertyInfo("SpecificHeat",gas,reagent.SpecificHeat);
            SetPropertyInfo("GasOverlayTexture",gas,"/Textures/_DV/Effects/atmospherics.rsi");
            SetPropertyInfo("Reagent",gas,reagent.ID);
        }


        _prototypeManager.RegisterKind(kinds);
    }
    private bool TryGetPropertyInfo(string s, out object p)
    {
        PropertyInfo? q = typeof(GasPrototype).GetProperty(s);
        if (q != null)
        {
            p = q;
            return true;
        }
        else
        {
            p = new object();
            return false;
        }
    }
    private void SetPropertyInfo(string s, object obj, object? val)
    {
        if (TryGetPropertyInfo(s, out object p) && p is PropertyInfo q)
        {
            q.SetValue(obj,val);
        }
    }
    private void OnBoiling(BoilingEvent args)
    {
        
    }
}