using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Cards;

namespace SWE1_MTCG.Battle
{
    public class BattleDeck : Deck
    {
        public BattleDeck(IEnumerable cards)
        {
            _cards = (List<Card>)cards;
        }

        public bool HasCardsLeft()
        {
            return _cards.Count > 0;
        }
    }
}
