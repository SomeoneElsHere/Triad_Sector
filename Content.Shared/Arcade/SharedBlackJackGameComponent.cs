using Robust.Shared.Serialization;

namespace Content.Shared.Arcade
{
    public abstract partial class SharedBlackJackGameComponent : Component
    {
        //OBJECTS//
        public abstract class BlackJackObjs
        {

        }

        public sealed class Card : BlackJackObjs
        {
            public enum CardColor
            {
                Red,
                Black,
            }

            public int value;

            public CardColor color;

            public Card(int val, bool isRed)
            {
                value = val;
                if (isRed)
                {
                    color = CardColor.Red;
                }
                else
                {
                    color = CardColor.Black;
                }
            }
        }


        //modifiers
        public abstract class Modifiers : BlackJackObjs
        {
            public Type? inp1F;
            public Type? inp2F;

            public Object? oupF;
            protected Type? oupTF;
            public String? Name;
            protected Func<object, object, object>? Function;
        }


        //COOL CARD DATA GOES HERE!!!!
        public abstract class CoolCards : Modifiers
        {
            public Type? inpP;
            protected Func<BlackJackAction, bool>? Prerequisites;
            //O should be the game state
            public void Apply(BlackJackAction P, object I1, object I2, ref object O)
            {
                if (Prerequisites != null && Prerequisites.Invoke(P))
                {
                    if (Function != null)
                    {
                        var oup = Function.Invoke(I1, I2);
                        if (!(oup is bool) && oupTF != null && O.GetType() == oupTF)
                        {
                            O = oup;
                        }
                    }
                }
            }
        }

        //copy and paste this to create the basis of a cool card
        public sealed class Jonkler : CoolCards
        {

            public Jonkler()
            {
                Name = "Jonkler";

                inpP = typeof(BlackJackGameState);

                Prerequisites = x =>
                {
                    if (x.GetType() == inpP)
                    {

                        return x != null && ((BlackJackGameState)x).handscore > 10;  //if handscore >10

                    }
                    return false;
                };

                inp1F = typeof(BlackJackGameState);
                oupTF = typeof(BlackJackGameState);

                Function = (x, y) =>
                {
                    if (x.GetType() == inp1F)
                    {
                        return ((BlackJackGameState)x).handscore += 10; //add 10 handscore
                    }
                    return false;
                };
            }


        }

        public sealed class OhGodNo : CoolCards
        {

            public OhGodNo()
            {
                Name = "OhGodNo";

                inpP = typeof(BlackJackGameState);

                Prerequisites = x =>
                {
                    if (x.GetType() == inpP)
                    {
                        if (((BlackJackGameState)x) == null)
                        {
                            return false;
                        }

                        var h = ((BlackJackGameState)x).ScoringHand;
                        if (h == null)
                        {
                            return false;
                        }

                        return h.Find((Card c) => c.value == 6) != null && h.Find((Card c) => c.value == 7) != null; //checks if the hand you just played includes the numbers.. oh dear lord.

                    }
                    return false;
                };

                inp1F = typeof(BlackJackGameState);
                oupTF = typeof(BlackJackGameState);

                Function = (x, y) =>
                {
                    if (x.GetType() == inp1F)
                    {
                        return ((BlackJackGameState)x).handscore += 100; //adds 100 to the score, I still hate you.
                    }
                    return false;
                };
            }


        }



        //wrapper and state of game data
        public abstract class BlackJackAction
        {
            public enum UIState
            {
                Playmat,
                Shop,
                Score,
            }

            public List<Card>? BenchHand;

            public List<Card>? ScoringHand;
            public List<Card>? CardPool;

            public List<CoolCards>? Cools;

            public int money;

            public int round;

            public int handscore;

            public int totalscore;

            public int scoreToBeat;

            private UIState UIstate;

            public BlackJackAction()
            {
                UIstate = UIState.Playmat;
                round = 1;
                handscore = 0;
                scoreToBeat = 50;
                totalscore = 0;
                money = 0;
                CardPool = new List<Card>();
                for(int i = 2; i<42; i++)
                {
                    CardPool.Add(new Card(i/2,i%2==0));
                }
                Cools = new List<CoolCards>();
                BenchHand = new List<Card>();
                ScoringHand = new List<Card>();
            }
        }

        //If you just want the current state of the game

        public sealed class BlackJackGameState : BlackJackAction
        {

        }

        //if a player action is wanted

        public sealed class BlackJackPlayerAction : BlackJackAction
        {

            public abstract class BasePlayerAction
            {
            }

            public sealed class PlayerActionSelect : BasePlayerAction
            {
                public int slot;

                public enum selectionRequest
                {
                    Buy,
                    Sell,
                    Play,
                    Discard,
                }

                public selectionRequest Request;
            }

            public sealed class PlayerPressButton : BasePlayerAction
            {
                public enum ButtonRequest
                {
                    PlayCards,
                    ExitShop,

                    Restart,
                }

                public ButtonRequest Request;
            }

        }

        //Internal Server events
        public sealed class BlackJackServerAction : BlackJackAction
        {
            public abstract class BaseServerAction
            {
            }

            public sealed class ServerUIEvent : BaseServerAction
            {
                public enum UISwitch
                {
                    Shop,
                    Playmat,
                    GameOver,
                }

                public UISwitch Switch;
            }

        }

        [Serializable, NetSerializable]
        public enum BlackJackGameUiKey
        {
            Key,
        }

        [Serializable, NetSerializable]
        public sealed class BlackJackPlayerActionMessage : BoundUserInterfaceMessage
        {
            public readonly BlackJackAction PlayerAction;
            public BlackJackPlayerActionMessage(BlackJackAction playerAction)
            {
                PlayerAction = playerAction;
            }
        }
    }
}
