using System;
using CardLib;

namespace Durak
{
    public enum HandType
    {
        attack = 0,
        defend
    }
    public sealed class Hand : PlayingCards, ICloneable
    {
        public HandType m_Type = HandType.attack;
        public Hand(HandType type)
        {
            m_Type = type;
        }
        public HandType GetMode()
        {
            return m_Type;
        }
        public Hand getFlush()
        {
            Hand outHand = new Hand(m_Type);
            Suit mostPrevalentSuit = GameUtil.FindMostPrevalentSuit(this);
            foreach (PlayingCard card in this)
            {
                if (mostPrevalentSuit == card.suit)
                {
                    outHand += card;
                }
            }
            return outHand;
        }
       
        /// <param name="cards">Hand</param>
        /// <param name="card">PlayingCard</param>
        public static Hand operator +(Hand cards, PlayingCard card)
        {
            if (!(cards.Contains(card)))
            {
                cards.Add(card);
            }
            else
            {
                throw new Exception("Unable to complete add->Hand");
            }
            return cards as Hand;
        }
        /// <param name="cards"></param>
        /// <param name="card"></param>
        public static Hand operator -(Hand cards, PlayingCard card)
        {
            if (cards.Count > 0)
            {
                if (cards.Contains(card))
                {
                    cards.Remove(card);
                }
            }
            return cards as Hand;
        }
        public override object Clone()
        {
            return MemberwiseClone();
        }

    }
}
