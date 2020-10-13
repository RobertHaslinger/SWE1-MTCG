using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SWE1_MTCG.Interfaces;

namespace SWE1_MTCG.Cards
{
    public class Deck : CardStack
    {

        public Card GetRandomCard()
        {
            if (!_cards.Any())
            {
                return null;
            }

            Random random = new Random();
            return _cards[random.Next(_cards.Count)];
        }

        public bool IsInDeck(Card card)
        {
            return _cards.Contains(card);
        }
    }
}
