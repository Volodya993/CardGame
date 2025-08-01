﻿using CardLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Durak
{
    public class Round : List<Turn>, ICloneable
    {
        private Game m_Game;
        public bool attackMode = true;
        private Turn currAttacker = null;
        private Turn currDefender = null;
        private int m_ID = 0;
        public int m_CurrentTurn = 0;
        
        public Round()
        {
            m_ID++;
        }
        public bool Initialize(Game game, Players players, int iLoserId =0)
        {
            m_Game = game;
            bool bRet = false;
            Player prevLosingPlayer = players[iLoserId];
            Add(new Turn(prevLosingPlayer, HandType.attack, Count));
            int i = 1;
            foreach (Player player in players)
            {
                if (prevLosingPlayer != player)
                {
                    Add(new Turn(player, (HandType)(i % 2), Count));
                    i++;
                }
            }
            if (PlayerType.computer == prevLosingPlayer.GetMode())
            {
                Game.btnDoneGuard = true;
                m_Game.Timed_Response(ResponseType.next_turn_bypass);
            }
            bRet = true;
            return bRet;
        }
        
        public void Expand()
        {
            if (Count <= m_CurrentTurn)
            {
                Turn newTurn = GetCurrentTurn();
                Add(newTurn++);
            }
        }
        public bool HasPlayerLost(Player player)
        {
            bool bRet = false;
            foreach (Turn turn in this)
            {
                if (turn.GetPlayer() == player)
                {
                    bRet = turn.isLoser();
                }
            }
            return bRet;
        }
        public Player FindLoser()
        {
            Player loser = null;
            foreach (Player player in Game.m_Players)
            {
                if (HasPlayerLost(player))
                {
                    loser = player;
                }
            }
            return loser;
        }
        public Turn GetCurrentTurn()
        {
            return this[m_CurrentTurn];
        }
     
        public void outputStatusToWindow(ref GameGui gg)
        {
            foreach (Turn turn in this)
            {
                Player myPlayer = turn.GetPlayer();
                switch (myPlayer.ID)
                {
                    case 0:
                        gg.lblPlayer0.Content = "Player 0 (" + ((PlayerType.computer==myPlayer.GetMode())?"computer":"human") + ") round " + GetHashCode().ToString() + " turn no. " + turn.GetHashCode().ToString();
                        Game.WriteToLog(gg.lblPlayer0.Content.ToString());
                        break;
                    case 1:
                        gg.lblPlayer1.Content = "Player 1 (" + ((PlayerType.computer == myPlayer.GetMode()) ? "computer" : "human") + ") round " + GetHashCode().ToString() + " turn no. " + turn.GetHashCode().ToString();
                        Game.WriteToLog(gg.lblPlayer1.Content.ToString());
                        break;
                    case 2:
                        gg.lblPlayer2.Content = "Player 2 (" + ((PlayerType.computer == myPlayer.GetMode()) ? "computer" : "human") + ") round " + GetHashCode().ToString() + " turn no. " + turn.GetHashCode().ToString();
                        Game.WriteToLog(gg.lblPlayer2.Content.ToString());
                        break;
                    case 3:
                        gg.lblPlayer3.Content = "Player 3 (" + ((PlayerType.computer == myPlayer.GetMode()) ? "computer" : "human") + ") round " + GetHashCode().ToString() + " turn no. " + turn.GetHashCode().ToString();
                        break;
                    case 4:
                        gg.lblPlayer4.Content = "Player 4 (" + ((PlayerType.computer == myPlayer.GetMode()) ? "computer" : "human") + ") round " + GetHashCode().ToString() + " turn no. " + turn.GetHashCode().ToString();
                        break;
                    case 5:
                        gg.lblPlayer5.Content = "Player 5 (" + ((PlayerType.computer == myPlayer.GetMode()) ? "computer" : "human") + ") round " + GetHashCode().ToString() + " turn no. " + turn.GetHashCode().ToString();
                        break;

                    default:
                        break;
                }
            }
        }
        public void GetAttacker()
        {
            uint attackerChoice = GetRandom.RangedRandom.GenerateUnsignedNumber(0, Convert.ToUInt32((this.Count<Turn>())-1), 0);
            currAttacker = this.ElementAt(Convert.ToInt32(attackerChoice));
            if (attackerChoice == 5)
            {
                currDefender = this.ElementAt(0);
            }
            else
            {
                currDefender = this.ElementAt((Convert.ToInt32(attackerChoice)) + 1);
            }
        }

        public int PlayerPlayOrder()
        {
            int iRet = 0;
            Round prevRound = (Round)Clone();
            prevRound--;
            if (null != prevRound)
            {
                Player prevLostPlayer = prevRound.FindLoser();
                if (null != prevLostPlayer)
                {
                    iRet = prevLostPlayer.ID;
                }
            }
            return iRet;
        }
        /// <param name="turn">Round</param>
        /// <returns>Round (may be null)</returns>
        public static Round operator --(Round round)
        {
            Round aRound = null;
            if (Game.m_Rounds.Count > 0)
            {
                int iRoundId = Game.m_Rounds[Game.m_Rounds.Count - 1].m_ID;

                if (round.m_ID > iRoundId)
                {
                    aRound = Game.m_Rounds[iRoundId];
                }
                else
                {
                    aRound = Game.m_Rounds[round.m_ID];
                }
            }
            return aRound;
        }
        /// <returns>object</returns>
        public object Clone()
        {
            Round round = new Round();
            round.m_ID--;//decrement as constructor always increments
            return round;
        }
        /// <returns>int</returns>
        public override int GetHashCode()
        {
            return m_ID;
        }
        /// <param name="turn"></param>
        /// <param name="gg"></param>
        public void updateBoldedStatus(Turn turn, ref GameGui gg)
        {
            gg.lblPlayer0.FontWeight = FontWeights.Normal;
            gg.lblPlayer1.FontWeight = FontWeights.Normal;
            gg.lblPlayer2.FontWeight = FontWeights.Normal;
            gg.lblPlayer3.FontWeight = FontWeights.Normal;
            gg.lblPlayer4.FontWeight = FontWeights.Normal;
            gg.lblPlayer5.FontWeight = FontWeights.Normal;
            switch (turn.GetPlayer().ID)
            {
                case 0:
                    gg.lblPlayer0.FontWeight = FontWeights.Bold;
                    break;
                case 1:
                    gg.lblPlayer1.FontWeight = FontWeights.Bold;
                    break;
                case 2:
                    gg.lblPlayer2.FontWeight = FontWeights.Bold;
                    break;
                case 3:
                    gg.lblPlayer3.FontWeight = FontWeights.Bold;
                    break;
                case 4:
                    gg.lblPlayer4.FontWeight = FontWeights.Bold;
                    break;
                case 5:
                    gg.lblPlayer5.FontWeight = FontWeights.Bold;
                    break;

                default:
                    break;
            }
        }
    }
}
