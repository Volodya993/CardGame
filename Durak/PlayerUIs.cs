using System;
using System.Collections.Generic;


namespace Durak
{
    public class PlayerUIs : List<PlayerUI>, ICloneable
    {
        public static int NumPlayers = 0;
        public PlayerUIs()
        {
        }

        public PlayerUIs(int numPlayers)
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
                    Add(new PlayerUI(i));
                }
                bRet = true;
            }
            catch (Exception ex)
            {

            }
            return bRet;
        }
        /// <param name="cards">PlayersUIs/param>
        public void CopyTo(PlayerUIs UIs)
        {
            for (int i = 0; i < this.Count; i++)
            {
                UIs[i] = this[i];
            }
        }
        /// <returns>Object</returns>
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
