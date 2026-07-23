using Content.Shared.Arcade;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.List;
using Robust.Shared.Serialization;

namespace Content.Server.Arcade.BlackJack;

[RegisterComponent]
public sealed partial class BlackJackGameComponent : SharedBlackJackGameComponent
{
    public BlackJackGame? Game;

    public EntityUid? Player = null;

    public readonly List<EntityUid> Spectators = new();
}
