using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Controller
{
    public class CardController : ControllerWithDbAccess
    {
        private ICardService _cardService;

        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        public KeyValuePair<StatusCode, object> CreateCard(Card card)
        {
            try
            {

                card= _cardService.CreateCard(card);
                return new KeyValuePair<StatusCode, object>(StatusCode.Created, card);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        public KeyValuePair<StatusCode, object> GetCards(List<Guid> guids)
        {
            List<Card> cards = new List<Card>();
            try
            {
                foreach (Guid guid in guids)
                {
                    cards.Add(_cardService.GetCard(guid));
                }

                if (!cards.Any())
                    return new KeyValuePair<StatusCode, object>(StatusCode.NoContent, "");

                return new KeyValuePair<StatusCode, object>(StatusCode.OK, cards);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
            
        }

        public KeyValuePair<StatusCode, object> DeleteCard(Guid guid)
        {
            try
            {
                Card card = _cardService.DeleteCard(guid);
                return new KeyValuePair<StatusCode, object>(StatusCode.OK, card);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }
    }
}
