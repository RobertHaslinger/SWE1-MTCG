using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Cards;

namespace SWE1_MTCG.Battle
{
    public class BattleDeck : Deck
    {
        public BattleDeck(IEnumerable<Card> cards) : base(cards)
        {
        }

        public bool HasCardsLeft()
        {
            return Cards.Count > 0;
        }
    }
}
