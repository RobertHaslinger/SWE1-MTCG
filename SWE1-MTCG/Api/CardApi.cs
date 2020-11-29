using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Controller;
using SWE1_MTCG.Dto;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Server;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Api
{
    public class CardApi : IRestApi
    {
        private CardController _cardController;
        public bool AllowAnonymous => true;

        public CardApi()
        {
            ICardService cardService= new CardService();
            _cardController= new CardController(cardService);
        }

        public ResponseContext Get(Dictionary<string, object> param)
        {
            throw new NotImplementedException();
        }

        public ResponseContext Post(Dictionary<string, object> param)
        {
            RequestContext request = (RequestContext) param["request"];
            if (!request.Headers.ContainsKey("Content-Type") || request.Headers["Content-Type"] != "application/json")
            {
                return new ResponseContext(request, new KeyValuePair<StatusCode, object>(StatusCode.UnsupportedMediaType, ""));
            }

            CardDto cardDto = JsonSerializer.Deserialize<CardDto>(request.Payload);
            Card card;
            if (cardDto==null || string.IsNullOrWhiteSpace(cardDto.CardType) || string.IsNullOrWhiteSpace(cardDto.Name) || (card=cardDto.ToObject())==null)
            {
                return new ResponseContext(request,
                    new KeyValuePair<StatusCode, object>(StatusCode.BadRequest,
                        "Either the card type or the name is empty or the given type does not exist"));
            }

            return new ResponseContext(request, _cardController.CreateCard(card));
        }

        public ResponseContext Put(Dictionary<string, object> param)
        {
            throw new NotImplementedException();
        }

        public ResponseContext Delete(Dictionary<string, object> param)
        {
            RequestContext request = (RequestContext)param["request"];
            Guid guid;
            if (!Guid.TryParse(request.RequestedResource, out guid))
            {
                return new ResponseContext(request, new KeyValuePair<StatusCode, object>(StatusCode.BadRequest, "The requested resource is no valid GUID"));
            }

            return new ResponseContext(request, _cardController.DeleteCard(guid));
        }
    }
}
