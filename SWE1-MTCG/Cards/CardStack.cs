using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Interfaces;

namespace SWE1_MTCG.Cards
{
    public class CardStack : ICardCollection
    {

        #region fields

        protected List<Card> _cards;
        #endregion

        #region properties

        #endregion

        #region constructor

        public CardStack()
        {
            _cards = new List<Card>();
        }
        #endregion

        #region private methods

        #endregion

        #region public methods

        public IEnumerable GetAllCards()
        {
            return _cards;
        }

        public void AddCard(Card card)
        {
            _cards.Add(card);
        }

        public void AddCards(IEnumerable cards)
        {
            if (cards == null)
            {
                return;
            }

            _cards.AddRange(cards as List<Card>);
        }

        #endregion

    }
}
