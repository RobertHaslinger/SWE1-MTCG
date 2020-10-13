using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SWE1_MTCG.Interfaces;

namespace SWE1_MTCG.Cards
{
    public class Deck : CardStack
    {


        #region fields

        #endregion

        #region properties

        #endregion

        #region constructor

        #endregion

        #region private methods

        #endregion

        #region public methods

        /// <summary>
        /// Try to add a card to the deck. A card can only be added if there is max. one duplicate already in the deck and the deck has not already 4 cards.
        /// </summary>
        /// <param name="card"></param>
        /// <returns>bool - whether the card was added or not.</returns>
        public override bool AddCard(Card card)
        {
            if (_cards.FindAll(c => c==card).Count >= 2)
            {
                return false;
            }

            if (_cards.Count >= 4)
            {
                return false;
            }

            _cards.Add(card);
            return true;
        }

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

        #endregion

    }
}
