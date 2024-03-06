using System;
using System.Collections.Generic;

namespace Durak
{
    class ComputerPlayers : List<ComputerPlayer>, ICloneable
    {
        public static int NumPlayers = 0;
        public ComputerPlayers(int numPlayers)
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
                for (int i = 0; i < NumPlayers; i++)
                {
                    Add(new ComputerPlayer(i));
                }
                bRet = true;
            }
            catch (Exception ex)
            {

            }
            return bRet;
        }
        /// <param name="cards">Players</param>
        public void CopyTo(ComputerPlayers players)
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
