/*
 * Author      : Group01
 * filename    : Turn.cs
 * Date        : 12-Apr-2018
 * Purpose     : This file is created as the requirement for the final project for OOP-4200.
 * Description : This is the turn class that is in charge of any player's turn
 */

using System;
using CardLib;

namespace Durak
{
    public class Turn : ICloneable
    {
        private bool m_bLoser = false;
        private int m_ID = 0;
        private Hand m_Hand = new Hand(HandType.attack);
        private Player m_Player = null;
        /// <param name="curTurnID">id number</param>
        public Turn(int curTurnID = 0)
        {
            m_ID = ++curTurnID;
        }
        /// <param name="player">player</param>
        /// <param name="curTurnID">id number</param>
        public Turn(Player player, int curTurnID = 0)
        {
            m_Player = player;
            m_ID = ++curTurnID;
        }
        /// <param name="player">player</param>
        /// <param name="curTurnID">id number</param>
        public Turn(Player player, HandType type, int curTurnID = 0)
        {
            m_Player = player;
            m_ID = ++curTurnID;
            m_Hand.m_Type = type;
        }
        /// <returns>bool</returns>
        public bool isLoser()
        {
            return m_bLoser;
        }
        public void SetLost()
        {
            m_bLoser = true;
        }
        /// <returns>bool</returns>
        public bool isDefending()
        {
            bool bRet = false;
            bRet = (HandType.defend == m_Hand.m_Type);
            return bRet;
        }
        /// <returns></returns>
        public Hand GetHand()
        {
            return m_Hand;
        }
        public void SetHand(Hand hand)
        {
            m_Hand = hand;
        }
        /// <param name="cards">PlayingCards (or Hand)</param>
        /// <returns>PlayingCards (or Hand)</returns>
        public PlayingCards Attack(PlayingCards cards)
        {
            foreach (PlayingCard card in cards)
            {
                m_Hand += card;
            }
            return m_Hand;
        }
        /// <param name="card">PlayingCard</param>
        /// <returns>PlayingCards (or Hand)</returns>
        public PlayingCards Attack(PlayingCard card)
        {
            m_Hand += card;
            return m_Hand;
        }
        /// <param name="card">PlayingCard</param>
        /// <returns>PlayingCards (or Hand)</returns>
        public PlayingCards Defend(PlayingCard card)
        {
            m_Hand += card;
            return m_Hand;
        }
        /// <returns></returns>
        public Player GetPlayer()
        {
            return m_Player;
        }
        /// <param name="turn">Turn</param>
        /// <returns>Turn (may be null)</returns>
        public static Turn operator ++(Turn turn)
        {
            int iTurnId = Game.m_Rounds[Game.m_Rounds.Count - 1].m_CurrentTurn;
            if (turn.m_ID > iTurnId)
            {
                return Game.m_Rounds[Game.m_Rounds.Count - 1][turn.m_ID];
            }
            else
            {
                return new Turn(turn.m_Player++,(turn.m_Hand.m_Type==HandType.attack)?HandType.defend:HandType.attack, iTurnId);
            }
        }
        /// <param name="turn">Turn</param>
        /// <returns>Turn (may be null)</returns>
        public static Turn operator --(Turn turn)
        {
            int iTurnId = Game.m_Rounds[Game.m_Rounds.Count - 1].m_CurrentTurn;//
            
            if (turn.m_ID > iTurnId)
            {
                return Game.m_Rounds[Game.m_Rounds.Count - 1][iTurnId - 1];
            }
            else
            {
                return Game.m_Rounds[Game.m_Rounds.Count - 1][turn.m_ID];
            }
        }
        /// <param name="obj">object (Turn implied)</param>
        /// <returns>-1, 0, 1</returns>
        public int CompareTo(object obj)
        {
            // test if it's turn
            if (obj is Turn)
            {
                if (Game.m_Players.Count < ((Turn)obj).m_ID)
                {
                    return (isLoser()) ? -1 : 1;
                }
                else
                {
                    return this.GetHashCode() - obj.GetHashCode();
                }
            }
            else
            {
                throw (new ArgumentException("Cannot compare Turn objects with objects of type {0}", obj.GetType().ToString()));
            }
        }
        /// <returns>object</returns>
        public object Clone()
        {
            Turn turn = new Turn(m_Player);
            turn.m_ID--;
            return turn;
        }
        /// <returns>int</returns>
        public override int GetHashCode()
        {
            return m_ID;
        }
    }
}
