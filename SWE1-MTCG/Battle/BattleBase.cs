using System;
using System.Collections.Generic;
using System.Text;

namespace SWE1_MTCG.Battle
{
    public class BattleBase
    {
        public User Player1 { get; private set; }
        public User Player2 { get; private set; }

        public BattleBase(User player1, User player2)
        {
            Player1 = player1;
            Player2 = player2;
        }
    }
}
