using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Dto
{
    public class PackageDto : Dto<Package>
    {
        public List<Guid> CardGuids { get; set; }
        public override Package ToObject()
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

            return cards.Count == CardGuids.Count ? new Package(cards) : null;
        }
    }
}
