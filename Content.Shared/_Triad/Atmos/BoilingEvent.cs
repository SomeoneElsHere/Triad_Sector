
using Content.Shared.Chemistry.Reagent;
using Robust.Shared.Serialization;

namespace Content.Shared.Tips;

[Serializable, NetSerializable]
public sealed class BoilingEvent : EntityEventArgs
{
    ReagentPrototype? reagent;
}