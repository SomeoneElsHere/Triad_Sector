using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Content.Client.Arcade.UI;
using Content.Client.Resources;
using Content.Shared.Arcade;
using Content.Shared.Input;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using Robust.Shared.Graphics;
using Robust.Shared.IoC;
using Robust.Shared.Localization;
using Robust.Shared.Maths;
using Robust.Shared.Utility;
using static Robust.Client.UserInterface.Controls.BoxContainer;

namespace Content.Client.Arcade
{
    public sealed class BlackJackGameMenu : DefaultWindow
    {
        public event Action<SharedBlackJackGameComponent.BlackJackAction>? OnPlayerAction;

        public event Action<SharedBlackJackGameComponent.BlackJackGameState>? OnGameStart;

        public SharedBlackJackGameComponent.BlackJackAction? Action;

        public SharedBlackJackGameComponent.BlackJackGameState? GameState;

        private bool hasStarted = false;

        private bool actionwaiting = false;

        private readonly PanelContainer Root = new PanelContainer();

        private readonly PanelContainer PlaymatRoot = new PanelContainer();
        private List<Label> CoolCards = new List<Label>();
        private List<Label> CardPool = new List<Label>();

        private List<Button> CardHand = new List<Button>();

        private Label? HandValue;

        private GridContainer rootPlaymat = new GridContainer
        {
            Columns = 3
        };

        //root
        private GridContainer CoolCardsPlaymatGrid = new GridContainer
        {
            Columns = 1
        };

        private GridContainer PlaymatGrid = new GridContainer
        {
            Columns = 1
        };

        private GridContainer CardsPlaymatGrid = new GridContainer
        {
            Columns = 10
        };
        //Playmat
        private GridContainer CardsValuePlaymatGrid = new GridContainer
        {
            Columns = 5
        };
        private GridContainer CardsHandPlaymatGrid = new GridContainer
        {
            Columns = 5
        };

        private readonly PanelContainer ShopRoot = new PanelContainer();
        private List<Button> NewCards = new List<Button>();

        private Label? Money;

        private GridContainer rootShop = new GridContainer
        {
            Columns = 3
        };

        private GridContainer ShopExistingCool = new GridContainer
        {
            Columns = 1
        };

        private GridContainer ShopNewCool = new GridContainer
        {
            Columns = 5
        };

        private GridContainer ShopMoney = new GridContainer
        {
            Columns = 1
        };

        private readonly PanelContainer EndRoot = new PanelContainer();

        private Label? FinalScore;
        private Button NewGame = new Button
        {
            Text = "New Game?"
        };



        public BlackJackGameMenu()
        {
            MinSize = SetSize = new Vector2(300, 225);
            Title = "BlackJackGame";
            //Playmat

            PlaymatGrid.AddChild(CardsValuePlaymatGrid);
            PlaymatGrid.AddChild(CardsHandPlaymatGrid);

            rootPlaymat.AddChild(CoolCardsPlaymatGrid);
            rootPlaymat.AddChild(PlaymatGrid);
            rootPlaymat.AddChild(CardsPlaymatGrid);

            PlaymatRoot.AddChild(rootPlaymat);

            //Shop
            rootShop.AddChild(ShopExistingCool);
            rootShop.AddChild(ShopNewCool);
            rootShop.AddChild(ShopMoney);

            ShopRoot.AddChild(rootShop);

            //End
            Contents.AddChild(Root);

        }

        public void CloseMenus()
        {
            Root.RemoveAllChildren();
        }

        public void StartBlackJackGame(SharedBlackJackGameComponent.BlackJackGameState State)
        {
            if (hasStarted)
            {
                return;
            }

            GameState = State;

            if (GameState != null && GameState.BenchHand != null && GameState.CardPool != null && GameState.Cools != null)
            {
                foreach (SharedBlackJackGameComponent.Card Card in GameState.CardPool)
                {
                    Color color = Card.color == SharedBlackJackGameComponent.Card.CardColor.Red ? Color.Red : Color.DarkViolet;
                    CardPool.Add(new Label
                    {
                        FontColorOverride = color,
                        Text = Card.value.ToString()
                    });
                }

            }
        }



    }
}
