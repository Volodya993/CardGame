using System;
using System.Linq;
using CardLib;

namespace Durak
{
    public enum PlayerType
    {
        human = 0,
        computer
    }
    public class Player : ICloneable
    {
        private PlayerType m_Type = PlayerType.computer;
        private uint playerid = 0;
        public int ID
        {
            get { return (int)playerid; }
            set
            {
                playerid = (uint)value;
            }
        }
        public string Name { get; private set; }
        public Hand m_Hand { get; set; }
        /// <param name="id">int</param>
        public Player(int id, PlayerType type)
        {
            ID = id;
            m_Type = type;
        }
        /// <param name="name">String - The player's name</param>
        public Player(String name)
        {
            Name = name;
            ID = ++Players.NumPlayers;
        }
        public PlayerType GetMode()
        {
            return m_Type;
        }
        /// <returns>object</returns>
        public object Clone()
        {
            return MemberwiseClone();
        }
        /// <returns></returns>
        public Hand Attack(Hand prevHand)
        {
            Hand attackHand = new Hand(HandType.attack);
            attackHand += this.m_Hand.GetLowestCard();
            //}
            //Suit mostPrevalentSuit = GameUtil.FindMostPrevalentSuit(Hand);
            
            //foreach (PlayingCard card in Hand)
            //{
            //    if (mostPrevalentSuit == card.suit)
            //    {
            //        attackHand += card;
            //    }
            //}
            
            //{

            //}
            //}
            //else
            //{
                
            //}
            return attackHand;
        }
        public Hand MakeDefense(Hand attackHand)
        {
            Hand defendHand = new Hand(HandType.defend);
            Hand tempHand = new Hand(HandType.defend);
            for (int i=0; i < m_Hand.Count; i++)
            {
                for (int j=0; j < attackHand.Count; j++)
                {
                    if (attackHand[j] < m_Hand[i] && !tempHand.Contains(m_Hand[i]))
                    {
                        tempHand.Add(m_Hand[i]);
                    }
                }
                if (tempHand>attackHand)
                {
                    defendHand = tempHand;
                    break;
                }
            }
            return defendHand;
        }
        /// <param name="Hand">attacking hand</param>
        /// <returns>bool</returns>
        public bool CanDefend(Hand attackCards)
        {
            bool bRet = false;
            int iCountTrumpsInLeftHand = GameUtil.GetCountTrumps(m_Hand);
            int iCountTrumpsInRightHand = GameUtil.GetCountTrumps(attackCards);
            if (iCountTrumpsInRightHand > 0)
            {
                Hand rightTrumps = GameUtil.getTrumps(attackCards);
                if (iCountTrumpsInLeftHand > 0)
                {
                    Hand leftTrumps = GameUtil.getTrumps(m_Hand);
                    if (iCountTrumpsInRightHand > iCountTrumpsInLeftHand)
                    {
                        bRet = GameUtil.CompareTrumps(leftTrumps, rightTrumps);
                    }
                    else if (iCountTrumpsInRightHand < iCountTrumpsInLeftHand)
                    {
                        bRet = GameUtil.CompareTrumps(rightTrumps, leftTrumps);
                    }
                    else
                    {
                        bRet = GameUtil.CompareHands(m_Hand, attackCards);
                    }
                }
                else
                {
                    bRet = GameUtil.CompareHands(m_Hand, attackCards);
                }
            }
            else if (iCountTrumpsInLeftHand > 0)
            {
                bRet = true;
            }
            else
            {
                int iMaxCountRanksInLeftHand = GameUtil.getCountMaxRanksInHand(m_Hand);
                int iMaxCountRanksInRightHand = GameUtil.getCountMaxRanksInHand(attackCards);
                if (iMaxCountRanksInLeftHand == iMaxCountRanksInRightHand)
                {
                    bRet = true;
                }
                else
                {
                    //first test for sets, otherwise a flush for both hands
                    //if (GameUtil.doHandSuitsMatch(attackCards, m_Hand))
                    //{
                    //    bRet = (m_Hand>attackCards);
                    //}
                    bRet = GameUtil.CompareHands(m_Hand, attackCards);
                }
            }
            
            return bRet;
        }

        public static Player operator ++(Player player)
        {
            if (player.ID < Game.m_Players.Count-1)
            {
                return Game.m_Players[player.ID + 1];
            }
            else
            {
                return Game.m_Players[0];
            }
        }
        /// <param name="attackHand"></param>
        public Hand Defend(Hand attackHand)
        {
            Hand defendHand = new Hand(HandType.defend);
            for (int attDex = 0; attDex < attackHand.Count(); attDex++)
            {
                for (int defDex = 0; defDex < this.m_Hand.Count(); defDex++)
                {
                    if (this.m_Hand.ElementAt(defDex) > attackHand.ElementAt(attDex) && defendHand.Count() != attackHand.Count())
                    {
                        defendHand += this.m_Hand.ElementAt(defDex);
                        defDex = this.m_Hand.Count() - 1;
                    }
                }
            }
            return defendHand;
        }
        /// <returns>int</returns>
        public override int GetHashCode()
        {
            return ID;
        }
        public void SetMode(PlayerType type)
        {
            m_Type = type;
        }
    }
}
