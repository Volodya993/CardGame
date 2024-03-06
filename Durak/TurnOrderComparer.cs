using System.Collections.Generic;

namespace Durak
{
    public class TurnOrderComparer : IComparer<Turn>
    {
        public int Compare(Turn prev, Turn cur)
        {
            return prev.CompareTo(cur);
        }
    }
}