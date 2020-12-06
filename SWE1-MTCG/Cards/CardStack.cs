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

        public IEnumerable<Card> GetAllCards()
        {
            return _cards;
        }

        public virtual bool AddCard(Card card)
        {
            _cards.Add(card);
            return true;
        }

        public virtual bool AddCards(IEnumerable<Card> cards)
        {
            _cards.AddRange(cards);
            return true;
        }

        public bool RemoveCard(Card card)
        {
            return _cards.Remove(card);
        }

        #endregion

    }
}
