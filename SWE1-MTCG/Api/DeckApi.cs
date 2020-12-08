using System;
using System.Collections.Generic;
using System.Linq;
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
    public class DeckApi : IRestApi
    {
        private CardController _cardController;
        private UserController _userController;
        public bool AllowAnonymous => false;

        public DeckApi()
        {
            ICardService cardService= new CardService();
            _cardController= new CardController(cardService);
            IUserService userService= new UserService();
            _userController= new UserController(userService);
        }

        public ResponseContext Get(Dictionary<string, object> param)
        {
            RequestContext request = (RequestContext)param["request"];
            MtcgClient client = (MtcgClient)param["client"];

            return new ResponseContext(request, _cardController.GetCards(client.User.Deck.GetAllCards().Select(c => c.Guid).ToList()));
        }

        public ResponseContext Post(Dictionary<string, object> param)
        {
            RequestContext request = (RequestContext)param["request"];
            MtcgClient client = (MtcgClient)param["client"];

            if (!request.Headers.ContainsKey("Content-Type") || request.Headers["Content-Type"] != "application/json")
            {
                return new ResponseContext(request, new KeyValuePair<StatusCode, object>(StatusCode.UnsupportedMediaType, ""));
            }

            if (client.User.Deck.GetAllCards().Any())
            {
                return new ResponseContext(request, new KeyValuePair<StatusCode, object>(StatusCode.Conflict, "Deck is already configured. Use PUT instead if you want to update"));
            }

            CardCollectionDto deckDto = JsonSerializer.Deserialize<CardCollectionDto>(request.Payload);
            deckDto.CardCollectionType = typeof(Deck);
            Deck deck;
            if (deckDto == null || deckDto.CardGuids.Count != 4 || (deck = deckDto.ToObject() as Deck) == null)
            {
                return new ResponseContext(request,
                    new KeyValuePair<StatusCode, object>(StatusCode.BadRequest,
                        "Either the deck does not contain 4 cards or some cards do not exist"));
            }

            return new ResponseContext(request, _userController.ConfigureDeck(ref client, deck));
        }

        public ResponseContext Put(Dictionary<string, object> param)
        {
            RequestContext request = (RequestContext)param["request"];
            MtcgClient client = (MtcgClient)param["client"];

            if (!request.Headers.ContainsKey("Content-Type") || request.Headers["Content-Type"] != "application/json")
            {
                return new ResponseContext(request, new KeyValuePair<StatusCode, object>(StatusCode.UnsupportedMediaType, ""));
            }

            CardCollectionDto deckDto = JsonSerializer.Deserialize<CardCollectionDto>(request.Payload);
            deckDto.CardCollectionType = typeof(Deck);
            Deck deck;
            if (deckDto == null || deckDto.CardGuids.Count != 4 || (deck = deckDto.ToObject() as Deck) == null)
            {
                return new ResponseContext(request,
                    new KeyValuePair<StatusCode, object>(StatusCode.BadRequest,
                        "Either the deck does not contain 4 cards or some cards do not exist"));
            }

            return new ResponseContext(request, _userController.ConfigureDeck(ref client, deck));
        }

        public ResponseContext Delete(Dictionary<string, object> param)
        {
            throw new NotImplementedException();
        }
    }
}
