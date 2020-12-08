using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SWE1_MTCG.Battle;
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

        public Deck()
        {
            
        }

        public Deck(IEnumerable<Card> cards) : base(cards)
        {
            
        }
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
            if (Cards.FindAll(c => c==card).Count >= 2)
            {
                return false;
            }

            if (Cards.Count >= 4)
            {
                return false;
            }

            Cards.Add(card);
            return true;
        }

        public Card GetRandomCard()
        {
            if (!Cards.Any())
            {
                return null;
            }

            Random random = new Random();
            return Cards[random.Next(Cards.Count)];
        }

        public void ClearDeck()
        {
            Cards.Clear();
        }

        public bool IsInDeck(Card card)
        {
            return Cards.Contains(card);
        }

        public BattleDeck GetBattleDeck()
        {
            return new BattleDeck(Cards);
        }

        #endregion

    }
}
