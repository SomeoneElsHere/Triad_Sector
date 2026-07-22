using Robust.Client.GameObjects;
using Robust.Client.UserInterface;
using Robust.Shared.GameObjects;
using Robust.Shared.ViewVariables;
using static Content.Shared.Arcade.SharedBlackJackGameComponent;

namespace Content.Client.Arcade.UI;

public sealed class BlackJackGameBoundUserInterface : BoundUserInterface
{
    [ViewVariables] private BlackJackGameMenu? _menu;

    public BlackJackGameBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    public void SendAction(BlackJackAction action)
    {
        SendMessage(new BlackJackPlayerActionMessage(action));
    }

    protected override void Open()
    {
        base.Open();

        _menu = this.CreateWindow<BlackJackGameMenu>();

    }

    protected override void ReceiveMessage(BoundUserInterfaceMessage message)
    {
    }
}
