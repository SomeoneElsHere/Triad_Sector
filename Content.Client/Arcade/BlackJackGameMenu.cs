using System.Numerics;
using Content.Shared.Arcade;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;

namespace Content.Client.Arcade
{
    public sealed class BlackJackGameMenu : DefaultWindow
    {
        public event Action<SharedBlackJackGameComponent.BlackJackAction>? OnPlayerAction;

        public BlackJackGameMenu()
        {
            MinSize = SetSize = new Vector2(300, 225);
            Title = "Hello";
        }

    }
}
