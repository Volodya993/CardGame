using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardLib
{
    public class CardOutOfRangeException : Exception
    {
        private PlayingCards deckContents;
        public PlayingCards DeckContents
        {
            get
            {
                return deckContents;
            }
        }
        public CardOutOfRangeException(PlayingCards srcDeckContents)
            : base("There are only 52 cards in the deck.")
        {
            deckContents = srcDeckContents;
        }
        
    }
    public class CardAlreadyInDeckException : Exception
    {
        private PlayingCards deckContents;
        public PlayingCards DeckContents
        {
            get
            {
                return deckContents;
            }
        }
        public CardAlreadyInDeckException(PlayingCards srcDeckContents)
            : base("The card is already in the deck.")
        {
            deckContents = srcDeckContents;
        }
    }
}
