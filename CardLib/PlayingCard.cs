using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace CardLib
{
    public class PlayingCard : Card, ICloneable, IComparable, IEquatable<PlayingCard>
    {
        public PlayingCard() : base(Suit.Club, Rank.Two, 13) { } // unused

        /// <param name="theSuit">enum</param>
        /// <param name="theRank">enum</param>
        /// <param name="bFaceUp">bool</param>
        public PlayingCard(Suit theSuit, Rank theRank, int suitSize, bool bFaceUp = true)
            : base(theSuit, theRank, suitSize)
        {
            isFaceup = bFaceUp;
        }

        private bool isFaceup;
        public bool Faceup
        {
            get { return isFaceup; }
            set
            {
                isFaceup = value;
            }
        }
        public static Rank baseRank = Rank.Ace;

        /// <returns>string</returns>
        public override string ToString()
        {
            return rank + "_of_" + suit + "s";
        }

        public static bool useTrumps = false;

        public static Suit trump = Suit.Club;

        public static bool isAceHigh = true;

        #region COMPARISONS AND RELATIONAL OPERATORS
        /// <param name="leftCard">PlayingCard</param>
        /// <param name="rightCard">PlayingCard</param>
        /// <returns>bool</returns>
        public static bool operator >(PlayingCard leftCard, PlayingCard rightCard)
        {
            if (leftCard.suit == rightCard.suit)
            {
                if (isAceHigh)
                {
                    if (leftCard.rank == Rank.Ace)
                    {
                        if (rightCard.rank == Rank.Ace)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (rightCard.rank == Rank.Ace)
                        {
                            return false;
                        }
                        else
                        {
                            return (leftCard.rank > rightCard.rank);
                        }
                    }
                }
                else
                {
                    return (leftCard.rank > rightCard.rank);
                }
            }
            else
            {
                if (useTrumps && (rightCard.suit == PlayingCard.trump))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        /// <param name="leftCard">PlayingCard</param>
        /// <param name="rightCard">PlayingCard</param>
        /// <returns>bool</returns>
        public static bool operator <(PlayingCard leftCard, PlayingCard rightCard)
        {
            return !(leftCard > rightCard);
        }
        /// <param name="leftCard">PlayingCard</param>
        /// <param name="rightCard">PlayingCard</param>
        /// <returns>bool</returns>
        public static bool operator >=(PlayingCard leftCard, PlayingCard rightCard)
        {
            if (leftCard.suit == rightCard.suit)
            {
                if (isAceHigh)
                {
                    if (leftCard.rank == Rank.Ace)
                    {
                        return true;
                    }
                    else
                    {
                        if (rightCard.rank == Rank.Ace)
                        {
                            return false;
                        }
                        else
                        {
                            return (leftCard.rank >= rightCard.rank);
                        }
                    }
                }
                else
                {
                    return (leftCard.rank >= rightCard.rank);
                }
            }
            else
            {
                if (useTrumps && (rightCard.suit == PlayingCard.trump))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        /// <param name="leftCard">PlayingCard</param>
        /// <param name="rightCard">PlayingCard</param>
        /// <returns>bool</returns>
        public static bool operator <=(PlayingCard leftCard, PlayingCard rightCard)
        {
            return !(leftCard >= rightCard);
        }
        /// <returns>int</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        /// <param name="obj">PlayingCard (implied)</param>
        /// <returns>int</returns>
        public int CompareTo(object obj)
        {
            if (obj is PlayingCard)
            {
                return this.GetHashCode() - obj.GetHashCode();
            }
            else
            {
                throw (new ArgumentException("Cannot compare PlayingCard objects with objects of type {0}", obj.GetType().ToString()));
            }
        }
        /// <param name="leftCard">PlayingCard (Card implied)</param>
        /// <param name="rightCard">PlayingCard (Card implied)</param>
        /// <returns>bool</returns>
        public static bool operator ==(PlayingCard leftCard, PlayingCard rightCard)
        {
            bool bRet = false;
            {
                bRet = leftCard.CardEquals(rightCard);
            }
            return bRet;
        }

        /// <param name="leftCard">PlayingCard</param>
        /// <param name="rightCard">PlayingCard</param>
        /// <returns>bool</returns>
        public static bool operator !=(PlayingCard leftCard, PlayingCard rightCard)
        {
            return !(leftCard == rightCard);
        }
        /// <param name="obj">PlayingCard (implied)</param>
        /// <returns>bool</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj);
        }
        /// <param name="obj">PlayingCard</param>
        /// <returns>bool</returns>
        public bool Equals(PlayingCard obj)
        {
            return this == (PlayingCard)obj;
        }
        /// <param name="obj">PlayingCard (implied)</param>
        /// <returns>bool</returns>
        protected override bool CardEquals(object obj)
        {
            return base.Equals(obj);
        }
        /// <param name="card">PlayingCard</param>
        /// <returns>PlayingCard (may be null)</returns>
        public static PlayingCard operator ++(PlayingCard card)
        {
            int offset = (int)suitSize - ((int)suitSize - (int)baseRank) - (int)((PlayingCard.isAceHigh) ? Rank.Two : Rank.Ace);
            int offsetSuitSize = (int)suitSize + offset;
            PlayingCard newCard = null;
            if (isAceHigh)
            {
                if ((int)card.rank >= offsetSuitSize)
                {
                    newCard = new PlayingCard(card.suit, Rank.Ace, suitSize, false);
                }
                else
                {
                    if ((int)card.rank == offsetSuitSize)
                    {
                        newCard = new PlayingCard(card.suit, Rank.Ace, suitSize, false);
                    }
                    else
                    {
                        if (card.rank == Rank.Ace)
                        {
                            newCard = new PlayingCard((Suit)((int)++(card.suit)), (Rank)baseRank, suitSize, false);
                        }
                        else
                        {
                            newCard = new PlayingCard(card.suit, (Rank)((int)++(card.rank)), suitSize, false);
                        }
                    }
                }
            }
            else
            {
                if ((int)card.rank >= offsetSuitSize)
                {
                    newCard = new PlayingCard((Suit)((int)++(card.suit)), Rank.Ace, suitSize, false);
                }
                else
                {
                    newCard = new PlayingCard(card.suit, (Rank)((int)++(card.rank)), suitSize, false);
                }
            }
            return newCard;
        }
        /// <param name="card">PlayingCard</param>
        /// <returns>PlayingCard (may be null)</returns>
        public static PlayingCard operator --(PlayingCard card)
        {
            PlayingCard newCard = null;
            if ((int)(card.rank - 1) < (int)baseRank)
            {
                if (!((int)(card.suit - 1) < 0))
                {
                    newCard = new PlayingCard((Suit)((int)--(card.suit)),
                        ((isAceHigh) ? Rank.Ace : Rank.King), suitSize, false);
                }
            }
            else
            {
                newCard = new PlayingCard(card.suit, (Rank)((int)--(card.rank)), suitSize, false);
            }
            return newCard;
        }
        
        #endregion
        /// <returns>object</returns>
        public override object Clone()
        {
            return MemberwiseClone();
        }
        public void Flip()
        {
            Faceup = !Faceup;
        }

    }
}
