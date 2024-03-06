using System;
using GetRandom;
namespace CardLib
{

    public sealed class Deck : PlayingCards, ICloneable
    {
        public event EventHandler LastCardDrawn;
        public event EventHandler CardDealt;

        private int m_Count = 0;
        public int DeckCount
        {
            get { return m_Count; }
            private set
            {
                m_Count = value;
            }
        }
        private int m_Flags = 227;
        public int Flags
        {
            get { return m_Flags; }
            private set
            {
                m_Flags = value;
            }
        }

        private int m_DeckSize = 35;
        public int DeckSize
        {
            get { return m_DeckSize; }
            private set
            {
                m_DeckSize = value;
            }
        }
        private int m_SuitSize = 9;
        public int SuitSize
        {
            get { return m_SuitSize; }
            private set
            {
                m_SuitSize = value;
            }
        }
        private int m_Entropy = 0;
        public int Entropy
        {
            get { return m_Entropy; }
            set
            {
                m_Entropy = value;
            }
        }
        public Deck()
        {
            PlayingCard.useTrumps = true;
            Initialize();
        }
        private void Initialize()
        { 
            if ((int)DeckFlags.Large == m_DeckSize)
                m_SuitSize = 13;
            PlayingCard.baseRank = (Rank)Util.CalculateBaseRank(m_SuitSize);
            PlayingCard newCard = new PlayingCard(Suit.Club, PlayingCard.baseRank, m_SuitSize);
            for (int i=0;i<=m_DeckSize;i++)
            {
                PlayingCard nextCard = (PlayingCard)newCard.Clone();
                Add(nextCard);
                newCard++;
            }
        }
        /// <param name="flags">int</param>
        public Deck(int flags)
        {
            m_Flags = flags;
            PlayingCard.useTrumps = (0 != ((short)flags & (int)DeckFlags.UseTrump));
            PlayingCard.isAceHigh = (0 != ((short)flags & (int)DeckFlags.AceHigh));
            m_DeckSize = ((short)flags & (int)DeckFlags.Large);
            m_SuitSize = ((int)DeckFlags.Small==m_DeckSize)?5:(Util.getbits(flags, 1, 1) == 0) ? 9 : 13; // get 3rd bit from right
            PlayingCard.trump = (Suit)((flags >> 9) & (byte)DeckFlags.TrumpSuit);
            Initialize();
        }
        public void Shuffle()
        {
            Clear();
            m_Count = 0;
            for (int Counter = 0; Counter <= m_DeckSize; Counter++)
            {
                PlayingCard pCard = null;
                do
                {
                    uint floor = (uint)PlayingCard.baseRank;
                    uint ceiling = (uint)Util.CalculateOffsetSuitSize(m_SuitSize);
                    uint myRank = 0;
                    uint mySuit = RangedRandom.GenerateUnsignedNumber(4, 0);
                    if (PlayingCard.isAceHigh && (int)DeckFlags.Large != m_DeckSize)
                    {
                        ceiling++;
                        floor--;
                        myRank = RangedRandom.GenerateUnsignedNumber(floor, ceiling, (uint)m_Entropy) + 1;
                    }
                    else
                    {
                        myRank = RangedRandom.GenerateUnsignedNumber(ceiling, (uint)m_Entropy) + 1;
                    }
                    pCard = new PlayingCard((Suit)mySuit, (Rank)myRank, m_SuitSize, true);
                    
                } while (IsCardAlreadyInDeck(pCard));
                Add(pCard);
                m_Count++;
            }
            if (PlayingCard.isAceHigh && (int)DeckFlags.Large != m_DeckSize)
            {
                foreach (PlayingCard card in this)
                {
                    if (14 == (uint)card.rank)
                    {
                        card.rank = Rank.Ace;
                    }
                }
            }
            Turnover();
        }
        /// <param name="card">PlayingCard</param>
        /// <returns>bool</returns>
        private bool IsCardAlreadyInDeck(PlayingCard card)
        {
            bool bRet = false;
            if (0 != Count)
            {
                bRet = Contains(card);
            }
            return bRet;
        }
        /// <param name="iIndex">int</param>
        /// <returns>Card object</returns>
        public PlayingCard DealCard(int iIndex) 
        {
            PlayingCard card = null;
            if (iIndex >= 0 && iIndex <= m_DeckSize)
            {
                if ((iIndex == (Count-1)) && (LastCardDrawn != null))
                {
                    LastCardDrawn(this, EventArgs.Empty);
                }
                card = (PlayingCard)this[iIndex].Clone();
                Remove(this[iIndex]); 
                m_Count--;
                if (CardDealt != null)
                {
                    CardDealt(this, EventArgs.Empty);
                }
            }
            else
            {
                throw new CardOutOfRangeException(Clone() as PlayingCards);
            }
            return card;
        }
        /// <returns>Object</returns>
        public override object Clone()
        {
            Deck deck = new Deck(m_Flags);
            return deck;
        }
        /// <param name="cards">Deck</param>
        /// <param name="card">PlayingCard</param>
        /// <returns>Deck</returns>
        public static Deck operator +(Deck cards, PlayingCard card)
        {
            int deckLen = ((short)cards.m_Flags & (int)DeckFlags.Large);
            int suitLen = ((int)DeckFlags.Small == deckLen) ? 5 : (Util.getbits(cards.m_Flags, 1, 1) == 0) ? 9 : 13;
            int rankBase = Util.CalculateBaseRank(suitLen);
            if (!(cards.Contains(card)))
            {
                if (!((cards.Count + 1) > deckLen))
                {
                    if (!((cards.getCountBySuit(card.suit) + 1) > suitLen))
                    {
                        if (!((int)card.rank < rankBase))
                        {
                            cards.Add(card);
                        }
                        else
                        {
                            throw new CardOutOfRangeException(cards);
                        }
                    }
                    else
                    {
                        throw new CardOutOfRangeException(cards);
                    }
                }
                else
                {
                    throw new CardOutOfRangeException(cards);
                }
            }
            else
            {
                throw new CardAlreadyInDeckException(cards);
            }
            return cards as Deck;
        }
        /// <param name="cards"></param>
        /// <param name="card"></param>
        public static Deck operator -(Deck cards, PlayingCard card)
        {
            if (cards.Count > 0)
            {
                if (cards.Contains(card))
                {
                    cards.Remove(card);
                }
            }
            return cards as Deck;
        }
    }
}
