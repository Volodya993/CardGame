namespace Durak
{
    class ComputerPlayer : Player
    {
        /// <param name="name"></param>
        public ComputerPlayer(int id) : base(id, PlayerType.computer)
        {
        }

        /// <summary>
        /// Gives the computer player some silly AI to attack
        /// </summary>
        /// <returns></returns>
        //public Hand Attack()
        //{
        //    Hand attackHand = new Hand(HandType.attack);
        //    attackHand += this.Hand.GetLowestCard();
        //    foreach (PlayingCard card in this.Hand)
        //    {
        //        //tricky stuff
        //    }
        //    return attackHand;
        //}

        /// <summary>
        /// Gives the computer player some silly AI for defense
        /// </summary>
        /// <param name="attackHand"></param>
        /// <returns></returns>
        //public Hand Defend(Hand attackHand)
        //{
        //    Hand defendHand = new Hand(HandType.defend);
        //    foreach (PlayingCard defenseCard in this.Hand)
        //    {
        //        foreach (PlayingCard attackCard in attackHand)
        //        {
        //            if (defenseCard > attackCard && defendHand.Count() < attackHand.Count())
        //            {
        //                defendHand += defenseCard;
        //            }
        //        }
        //    }
        //    return defendHand;
        //}

        /// <summary>
        /// Allows computer to check if they have the ability to attack
        /// during a round between two other plauers.
        /// </summary>
        /// <returns></returns>
        ////private bool CanAttack()
        ////{
        ////}
    }
}
