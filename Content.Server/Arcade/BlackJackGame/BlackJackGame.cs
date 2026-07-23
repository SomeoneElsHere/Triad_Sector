using static Content.Shared.Arcade.SharedBlackJackGameComponent;
using Robust.Server.GameObjects;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Random;

namespace Content.Server.Arcade.BlackJack;

public sealed partial class BlackJackGame
{
    //required
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    private readonly ArcadeSystem _arcadeSystem;
    private readonly UserInterfaceSystem _uiSystem;
    private BlackJackAction? BJAction;

    private BlackJackGameState? BJState;

    [ViewVariables]
    private readonly EntityUid _owner = default!;

    public BlackJackGame(EntityUid owner)
    {
        IoCManager.InjectDependencies(this);
        _arcadeSystem = _entityManager.System<ArcadeSystem>();
        _uiSystem = _entityManager.System<UserInterfaceSystem>();

        _owner = owner;

        BJState = new BlackJackGameState();
    }
}
