﻿using System;
using System.Collections.Generic;
using System.Linq;
using CardLib;
namespace Durak
{
    public class Players : List<Player>, ICloneable
    {

        public static int NumPlayers = 0;
        public Players(int numPlayers)
        {
            NumPlayers = numPlayers;
            if (!Initialize())
                throw new Exception();
        }
        public bool Initialize()
        {
            bool bRet = false;
            try
            {
                Add(new Player(0, PlayerType.human));
                for (int i = 1; i < NumPlayers; i++)
                {
                    Add(new Player(i, PlayerType.computer));
                }
                bRet = true;
            }
            catch (Exception ex)
            {

            }
            return bRet;
        }
        public bool DealHands(int chosenNumPlayers, int handSize, ref Deck deck, bool bInit = false)
        {
            bool bRet = true;
            foreach (Player player in this)
            {
                if (bInit)
                    player.m_Hand = new Hand(HandType.attack);
                while (player.m_Hand.Count() < handSize)
                {
                    if (!Game.m_bDeckOutFlag)
                    {
                        player.m_Hand.Add(deck.DealCard(0));
                        bRet = true;
                    }
                    else
                    {
                        bRet = false;
                        break;
                    }
                }
            }
            return bRet;
        }
        /// <param name="cards">Players</param>
        public void CopyTo(Players players)
        {
            for (int i = 0; i < this.Count; i++)
            {
                players[i] = this[i];
            }
        }
        /// <returns>Object</returns>
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
