using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardLib
{
    public static class Util
    {
        /// <param name="suitSize">int</param>
        /// <returns>int</returns>
        public static int CalculateBaseRank(int suitSize)
        {
            int iBaseRank = 0;
            if (suitSize == 9)
            {
                iBaseRank = 6;
                PlayingCard.isAceHigh = true;
            }
            else if (suitSize == 5)
            {
                iBaseRank = 10;
                PlayingCard.isAceHigh = true;
            }
            else
            {
                iBaseRank = (int)((PlayingCard.isAceHigh) ? Rank.Two : Rank.Ace);
            }
            return iBaseRank;
        }
        /// <param name="suitSize">number of ranks in suit</param>
        /// <returns>int</returns>
        public static int CalculateOffsetSuitSize(int suitSize)
        {
            int offset = suitSize - (suitSize - (int)PlayingCard.baseRank) - (int)((PlayingCard.isAceHigh) ? Rank.Two : Rank.Ace);
            int offsetSuitSize = suitSize + offset;
            return offsetSuitSize;
        }
        public static int CalculateInitialHandSize(int suitSize, int numPlayers)
        {
            int iRet = 0;
            switch (numPlayers)
            {
                case 2:
                    { iRet = 6; break; }
                case 3:
                    { iRet = 6; break; }
                case 4:
                    { iRet = (suitSize > 5) ? 6 : 5; break; }
                case 5:
                    { iRet = (suitSize > 5) ? 6: 4; break; }
                case 6:
                    { iRet = (suitSize > 5) ? 6 : 3; break; }
                default:
                    break;
            }
            return iRet;
        }
        /// <param name="x">input number</param>
        /// <param name="p">position to search from</param>
        /// <param name="n">number of bits to find</param>
        /// <returns>int</returns>
        public static int getbits(int x, int p, int n)
        {
            return (x >> (p + 1)) & ~(~0 << n);
        }
        public static String SuitToStr(Suit suit)
        {
            String str = "";
            switch (suit)
            {
                case Suit.Club:
                    str = "club";
                    break;
                case Suit.Diamond:
                    str = "diamond";
                    break;
                case Suit.Heart:
                    str = "heart";
                    break;
                case Suit.Spade:
                    str = "spade";
                    break;
                default:
                    break;
            }
            return str;
        }
    }
}
