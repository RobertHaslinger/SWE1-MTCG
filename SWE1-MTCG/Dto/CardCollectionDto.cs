using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Dto
{
    public class CardCollectionDto : Dto<ICardCollection>
    {
        public List<Guid> CardGuids { get; set; }
        public Type CardCollectionType { get; set; }

        public override ICardCollection ToObject()
        {
            ICardService cardService= new CardService();
            List<Card> cards= new List<Card>();

            foreach (Guid guid in CardGuids)
            {
                Card card;
                if ((card = cardService.GetCard(guid)) != null)
                {
                    cards.Add(card);
                }
            }

            if (!CardCollectionType.IsAssignableTo(typeof(ICardCollection)))
            {
                return null;
            }

            return cards.Count == CardGuids.Count ? (ICardCollection)Activator.CreateInstance(CardCollectionType, cards) : null;
        }
    }
}
