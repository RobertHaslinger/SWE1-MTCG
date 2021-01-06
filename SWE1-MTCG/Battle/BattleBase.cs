using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SWE1_MTCG.Client;

namespace SWE1_MTCG.Battle
{
    public class BattleBase
    {
        public MtcgClient Player1 { get; private set; }
        public MtcgClient Player2 { get; private set; }

        public BattleBase(ref MtcgClient player1, ref MtcgClient player2)
        {
            Player1 = player1;
            Player2 = player2;
        }

        public bool CheckDecks()
        {
            return Player1.User.Deck.GetAllCards().Count() == 4 && Player2.User.Deck.GetAllCards().Count() == 4;
        }
    }
}
