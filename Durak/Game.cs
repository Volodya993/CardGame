using CardLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.IO;

namespace Durak
{
    public enum ResponseType
    {
        clear_all = 0,
        next_turn,
        next_turn_bypass
    }
    public class Game
    {
        private static String filePath = ("./logs/Game" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt");
        public Deck m_Deck = null;
        public Deck discardDeck = new Deck(0);
        public static bool m_bDeckOutFlag = false;
        public static Players m_Players = null;
        public GameGui m_Gg = null;
        public static List<Round> m_Rounds = new List<Round>();
        public static bool btnDoneGuard = false;
        public int m_HandSize = 0;
        DispatcherTimer m_DispatcherTimer = null;
        /// <param name="numPlayers"></param>
        /// <param name="gameFlags"></param>
        /// <param name="gg"></param>
        public Game(int numPlayers, int gameFlags, GameGui gg)
        {
            m_Gg = gg;
            Initialize(numPlayers, gameFlags);
            m_Gg.SetupGui();
            m_Gg.UpdateDisplay(m_Players);
            m_Gg.UpdateElements(m_Players);
            Round rnd = new Round();
            rnd.Initialize(this, m_Players);
            m_Rounds.Add(rnd);
        }
        /// <param name="chosenNumPlayers"></param>
        /// <param name="gameFlags"></param>
        public bool Initialize(int chosenNumPlayers, int gameFlags)
        {
            bool bRet = false;
            int trumpsuit = ((gameFlags >> 9) & (byte)DeckFlags.TrumpSuit);
            m_Deck = new Deck(gameFlags);
            m_Deck.LastCardDrawn += new System.EventHandler(m_Gg.Deck_LastCardDrawn);
            m_Deck.CardDealt += new System.EventHandler(m_Gg.Deck_CardDealt);
            m_Deck.Shuffle();
            m_Gg.gbxDeck.Header = "Deck: " + m_Deck.DeckCount + " cards";
            m_Players = new Players(chosenNumPlayers);
            m_HandSize = Util.CalculateInitialHandSize(m_Deck.SuitSize, chosenNumPlayers);
            m_Players.DealHands(chosenNumPlayers, m_HandSize, ref m_Deck, true);
            return bRet;
        }
        /// <param name="sender"></param>
        public void CardBoxClicked(object sender)
        {
            Turn curTurn = m_Rounds[m_Rounds.Count - 1].GetCurrentTurn();
            Turn prevTurn = (Turn)curTurn.Clone();
            prevTurn--;
            CardBox box = (CardBox)sender;
            PlayingCard selectedCard = (PlayingCard)(box).Card.Clone();
            StackPanel sp = (StackPanel)box.Parent;
            Grid g = (Grid)sp.Parent;
            PlayerUI pui = (PlayerUI)g.Parent;
            pui.DeleteCard(box);
            if (curTurn.isDefending())
            {
                m_Gg.m_BattleArea.AddDefenseCard(new CardBox(selectedCard, Orientation.Vertical, curTurn.GetHashCode()));
                curTurn.Defend(selectedCard);
                WriteToLog("Player " + prevTurn.GetPlayer().ID.ToString() + " " + ((!(PlayerType.human == prevTurn.GetPlayer().GetMode())) ? "computer" : "human")
                                    + " defended using " + selectedCard.ToString());
            }
            else
            {
                m_Gg.m_BattleArea.AddAttackCard(new CardBox(selectedCard, Orientation.Vertical, curTurn.GetHashCode()));
                curTurn.Attack(selectedCard);
                WriteToLog("Player " + prevTurn.GetPlayer().ID.ToString() + " " + ((!(PlayerType.human == prevTurn.GetPlayer().GetMode())) ? "computer" : "human")
                                    + " attacked using " + selectedCard.ToString());
            }
            curTurn.GetPlayer().m_Hand -= selectedCard;
            m_Rounds[m_Rounds.Count - 1].updateBoldedStatus(curTurn, ref m_Gg);
            btnDoneGuard = false;
        }
        /// <param name="bClickedByHuman"></param>
        public void DoneClicked(bool bClickedByHuman)
        {
            Round curRound = m_Rounds[m_Rounds.Count - 1];
            curRound.Expand();
            Turn curTurn = curRound.GetCurrentTurn();
            Turn prevTurn = (Turn)curTurn.Clone();
            prevTurn--;
            Player curPlayer = curTurn.GetPlayer();
            Player prevPlayer = prevTurn.GetPlayer();
            Hand prevHand = prevTurn.GetHand();
            Hand curHand = curTurn.GetHand();
            if (curTurn.GetPlayer().ID == prevTurn.GetPlayer().ID)
            {
                if (!curTurn.isDefending())
                {
                    if (!bClickedByHuman)
                    {
                        curHand = curPlayer.Attack(prevHand);
                        foreach (PlayingCard card in curHand)
                        {
                            m_Gg.m_BattleArea.AddAttackCard(new CardBox(card, Orientation.Vertical, curTurn.GetHashCode()));
                            curPlayer.m_Hand -= card;
                            WriteToLog("Player " + prevTurn.GetPlayer().ID.ToString() + " " + ((!(PlayerType.human == prevTurn.GetPlayer().GetMode())) ? "computer" : "human")
                                    + " attacked using " + card.ToString());
                        }
                        curTurn.SetHand(curHand);
                        if (0 == curHand.Count)
                        {
                            WriteToLog("Player " + prevTurn.GetPlayer().ID.ToString() + " " + ((!(PlayerType.human == prevTurn.GetPlayer().GetMode())) ? "computer" : "human")
                                    + " player (prev turn) sucessfully attacked/defended");
                        }
                        m_Gg.UpdateElements(m_Players);
                    }
                }
                if (false == btnDoneGuard)
                {
                    Timed_Response(ResponseType.next_turn);
                }
            }
            else
            {
                int iLoserId = 0;
                if (!bClickedByHuman)
                {
                    if (false == btnDoneGuard)
                    {
                        if (curTurn.GetPlayer().CanDefend(prevHand))
                        {
                            curHand = curPlayer.MakeDefense(prevHand);
                            foreach (PlayingCard card in curHand)
                            {
                                m_Gg.m_BattleArea.AddDefenseCard(new CardBox(card, Orientation.Vertical, curTurn.GetHashCode()));
                                curPlayer.m_Hand -= card;
                                WriteToLog("Player " + prevTurn.GetPlayer().ID.ToString() + " " + ((!(PlayerType.human == prevTurn.GetPlayer().GetMode())) ? "computer" : "human")
                                    + " defended using " + card.ToString());
                            }
                            curTurn.SetHand(curHand);
                            if (0 == curHand.Count)
                            {
                                WriteToLog("Player " + prevTurn.GetPlayer().ID.ToString() + " " + ((!(PlayerType.human == prevTurn.GetPlayer().GetMode())) ? "computer" : "human")
                                        + " player (prev turn) sucessfully attacked/defended");
                                iLoserId = prevTurn.GetPlayer().ID;
                            }
                        }
                        else
                        {
                            iLoserId = ProcessLoser(prevTurn, prevHand, prevPlayer.ID, curTurn, curHand, bClickedByHuman);
                        }
                    }
                }
                int ranksInPrevHand = GameUtil.getCountMaxRanksInHand(prevHand);
                int ranksInCurHand = GameUtil.getCountMaxRanksInHand(curHand);
                bool bTemp = GameUtil.doHandSuitsMatch(prevHand, curHand);
                if (ranksInPrevHand == ranksInCurHand)
                {
                    if (false == btnDoneGuard)
                    {
                        if (prevHand > curHand)
                        {
                            iLoserId = ProcessLoser(prevTurn, prevHand, prevPlayer.ID, curTurn, curHand, bClickedByHuman);
                        }
                        else if (curHand > prevHand)
                        {
                            iLoserId = ProcessLoser(curTurn, curHand, curPlayer.ID, prevTurn, prevHand, bClickedByHuman);
                        }
                        else
                        {
                            MessageBox.Show("Draw");
                        }
                    }
                }
                else
                {
                    if (curTurn.isDefending())
                    {
                        if (bClickedByHuman)
                        { 
                            if (GameUtil.ShallIDoThisForYou("You did not place the correct amount of cards in the battle area. Forfeit?"))
                            {
                                WriteToLog("Player " + prevTurn.GetPlayer().ID.ToString() + " " + ((!(PlayerType.human == prevTurn.GetPlayer().GetMode())) ? "computer" : "human")
                                        + " player (prev turn) sucessfully attacked/defended");
                                iLoserId = prevTurn.GetPlayer().ID;
                            }
                        }
                    }
                }
                m_Gg.UpdateElements(m_Players);
                DoneWithRound(iLoserId);
                curRound.m_CurrentTurn--;
            }
            curRound.m_CurrentTurn++;
            curRound.outputStatusToWindow(ref m_Gg);
            curRound.updateBoldedStatus(curRound.GetCurrentTurn(), ref m_Gg);
        }

        /// <param name="leftTurn"></param>
        /// <param name="leftHand"></param>
        /// <param name="playerId"></param>
        /// <param name="rightTurn"></param>
        /// <param name="rightHand"></param>
        /// <param name="bRightHuman"></param>
        int ProcessLoser(Turn leftTurn, Hand leftHand, int playerId, Turn rightTurn, Hand rightHand, bool bRightHuman)
        {
            int iLoserId = 0;
            String tempMessage = ("Player " + playerId.ToString() + " " + ((!bRightHuman) ? "computer" : "human")
                                    + " player sucessfully attacked/defended");
            MessageBox.Show(tempMessage);
            WriteToLog(tempMessage);
            leftTurn.SetLost();
            foreach (PlayingCard card in leftHand)
            {
                //rightTurn.GetPlayer().Hand.Add(card);
                discardDeck.Add(card);
                leftTurn.GetPlayer().m_Hand.Remove(card);
            }
            foreach (PlayingCard card in rightHand)
            {
                rightTurn.GetPlayer().m_Hand.Add(card);
            }
            iLoserId = playerId;
            return iLoserId;
        }
        /// <param name="type"></param>
        public void Timed_Response(ResponseType type)
        {
            m_DispatcherTimer = new DispatcherTimer();

            if (ResponseType.clear_all == type)
            {
                m_DispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                m_DispatcherTimer.Tick += new EventHandler(m_Gg.dtDestroy_Tick);
            }
            else if (ResponseType.next_turn == type)
            {
                m_DispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                m_DispatcherTimer.Tick += new EventHandler(m_Gg.dtNext_Tick);
            }
            else if (ResponseType.next_turn_bypass == type)
            {
                m_DispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                m_DispatcherTimer.Tick += new EventHandler(m_Gg.dtBypass_Tick);
            }
            
            m_DispatcherTimer.Start();
        }
        /// <param name="iLoserId"></param>
        public void DoneWithRound(int iLoserId)
        {
            Timed_Response(ResponseType.clear_all);
            Round rnd = new Round();
            rnd.Initialize(this, m_Players, iLoserId);
            m_Rounds.Add(rnd);
        }
        /// <param name="text"></param>
        public static void WriteToLog(String text)
        {
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.Write(Environment.NewLine + text);
            }
        }
        public void ForfeitTurn()
        {
            
            Round curRound = m_Rounds[m_Rounds.Count - 1];
            Turn curTurn = curRound.GetCurrentTurn();
            Turn prevTurn = (Turn)curTurn.Clone();
            prevTurn--;
            bool bHuman = (PlayerType.human == prevTurn.GetPlayer().GetMode());
            WriteToLog("Player " + prevTurn.GetPlayer().ID.ToString() + " " + ((!bHuman) ? "computer" : "human")
                                    + " player (prev turn) sucessfully attacked/defended");
            DoneWithRound(prevTurn.GetPlayer().ID);
        }

        /// <param name="attackHand"></param>
        public bool IsValidAttackHand(PlayingCards attackHand)
        {
            bool isValid = false;
            if (attackHand.Count() > 1)
            {
                for (int index = 0; index < attackHand.Count(); index++)
                {
                    if (index != 0)
                    {
                        if (attackHand.ElementAt(index - 1).rank == attackHand.ElementAt(index).rank)
                        {
                            isValid = true;
                        }
                    }
                }
            }
            else
            {
                isValid = true;
            }
            return isValid;
        }
        /// <param name="defenseHand"></param>
        /// <param name="attackHand"></param>
        public bool IsValidDefenseHand(PlayingCards defenseHand, PlayingCards attackHand)
        {
            bool isValid = false;
            defenseHand.Sort((c1,c2) => c1.suit.CompareTo(c2.suit));
            attackHand.Sort((c1, c2) => c1.suit.CompareTo(c2.suit));
            if (attackHand.Count() == defenseHand.Count())
            {
                for (int index = 0; index < attackHand.Count(); index++)
                {
                    if (defenseHand.ElementAt(index).suit == attackHand.ElementAt(index).suit)
                    {
                        if (defenseHand.ElementAt(index).rank > attackHand.ElementAt(index).rank)
                        {
                            isValid = true;
                        }
                        else
                        {
                            isValid = false;
                        }
                    }
                    else
                    {
                        isValid = false;
                    }
                }
            }
            return isValid;
        }
    }
}