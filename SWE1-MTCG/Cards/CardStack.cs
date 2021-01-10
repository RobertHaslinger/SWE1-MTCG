using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Cards
{
    public class CardStack : ICardCollection
    {

        #region fields

        protected List<Card> Cards;
        #endregion

        #region properties

        #endregion

        #region constructor

        public CardStack()
        {
            Cards = new List<Card>();
        }

        public CardStack(IEnumerable<Card> cards)
        {
            Cards = cards.ToList();
        }
        #endregion

        #region private methods

        #endregion

        #region public methods

        public IEnumerable<Card> GetAllCards()
        {
            return Cards;
        }

        public virtual bool AddCard(Card card)
        {
            Cards.Add(card);
            return true;
        }

        public bool AddCard(Guid cardId)
        {
            var cardService= new CardService();
            var card = cardService.GetCard(cardId);
            Cards.Add(card);
            return true;
        }

        public virtual bool AddCards(IEnumerable<Card> cards)
        {
            Cards.AddRange(cards);
            return true;
        }

        public bool RemoveCard(Card card)
        {
            return Cards.Remove(card);
        }

        public bool RemoveCard(Guid card)
        {
            return Cards.RemoveAll(c => c.Guid == card) > 0;
        }

        #endregion

    }
}
