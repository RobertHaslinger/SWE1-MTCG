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
