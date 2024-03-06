using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardLib
{

    public abstract class Card : ICloneable
    {
        public Suit suit;
        public Rank rank;
        public static int suitSize;

        public Card(Suit suitIn, Rank rankIn, int suitSizeIn)
        {
            suit = suitIn;
            rank = rankIn;
            suitSize = suitSizeIn;
        }

        public Card()
        {
            suit = 0;
            rank = 0;
            suitSize = 13;
        }

        public override string ToString()
        {
            return "The " + rank + " of " + suit + "s" ;
        }
        public abstract object Clone();
        #region COMPARISON AND RELATIONAL OPERATORS
        /// <param name="leftCard">Card</param>
        /// <param name="rightCard">Card</param>
        /// <returns>bool</returns>
        public static bool operator ==(Card leftCard, Card rightCard)
        {
            return (leftCard.suit == rightCard.suit) && (leftCard.rank == rightCard.rank);
        }
        /// <param name="leftCard">Card</param>
        /// <param name="rightCard">Card</param>
        /// <returns>bool</returns>
        public static bool operator !=(Card leftCard, Card rightCard)
        {
            return !(leftCard == rightCard);
        }
        /// <param name="obj">Card (implied)</param>
        /// <returns>bool</returns>
        public override bool Equals(object obj)
        {
            return this == (Card)obj;
        }
        /// <param name="obj">Card (implied)</param>
        /// <returns>bool</returns>
        protected abstract bool CardEquals(object obj);
        /// <returns>int</returns>
        public override int GetHashCode()
        {
            return suitSize * (int)suit + (int)rank;
        }
        #endregion
    }
}
