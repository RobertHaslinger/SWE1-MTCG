using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Client;
using SWE1_MTCG.Controller;
using SWE1_MTCG.Dto;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Server;
using SWE1_MTCG.Services;
using SWE1_MTCG.Trade;

namespace SWE1_MTCG.Api
{
    public class TradeApi : IRestApi
    {
        private MarketController _marketController;
        private CardController _cardController;
        public bool AllowAnonymous => false;

        public TradeApi()
        {
            _marketController= new MarketController(new MarketService());
            _cardController= new CardController(new CardService());
        }

        public ResponseContext Get(Dictionary<string, object> param)
        {
            RequestContext request = (RequestContext)param["request"];
            MtcgClient client = (MtcgClient)param["client"];
            if (!request.QueryParams.ContainsKey("username"))
            {
                return new ResponseContext(request, _marketController.GetAllOpenTradingDeals());
            }

            return new ResponseContext(request, _marketController.GetOpenTradingDealsForUser(request.QueryParams["username"]));
        }

        public ResponseContext Post(Dictionary<string, object> param)
        {
            RequestContext request = (RequestContext)param["request"];
            MtcgClient client = (MtcgClient)param["client"];

            //trade with existing deal
            if (!string.IsNullOrWhiteSpace(request.RequestedResource))
            {
                TradingDeal pendingDeal = null;
                if (!_marketController.TradingDealExists(request.RequestedResource, out pendingDeal))
                    return new ResponseContext(request, new KeyValuePair<StatusCode, object>(StatusCode.BadRequest, $"There is no open deal with the given id {request.RequestedResource}"));

                if (pendingDeal.PublisherId==client.User.UserId)
                    return new ResponseContext(request, new KeyValuePair<StatusCode, object>(StatusCode.BadRequest, "You can't trade with yourself"));

                Card cardToTrade = null;
                Guid cardGuid;
                if (string.IsNullOrWhiteSpace(request.Payload) || !Guid.TryParse(request.Payload, out cardGuid) ||
                    (cardToTrade=client.User.Stack.GetAllCards().FirstOrDefault(c => c.Guid.Equals(cardGuid)))==null)
                {
                    return new ResponseContext(request, new KeyValuePair<StatusCode, object>(StatusCode.BadRequest, "You must provide a card id that you possess and want to trade"));
                }

                return new ResponseContext(request, _marketController.ProcessTrade(pendingDeal, cardToTrade, client));
            }

            //create new deal
            TradingDeal deal = JsonSerializer.Deserialize<TradingDeal>(request.Payload);
            if (deal == null || string.IsNullOrWhiteSpace(deal.RequestedElement) || string.IsNullOrWhiteSpace(deal.RequestedType))
            {
                return new ResponseContext(request,
                    new KeyValuePair<StatusCode, object>(StatusCode.BadRequest,
                        "All properties must be set: CardId, RequestedElement, MinimumDamage, RequestedType"));
            }
            if (!client.User.Stack.GetAllCards().Any(c => c.Guid==deal.CardId))
                return new ResponseContext(request,
                    new KeyValuePair<StatusCode, object>(StatusCode.BadRequest,
                        "Nice try, but you don't have this card ;)"));
            deal.PublisherId = client.User.UserId;
            return new ResponseContext(request, _marketController.AddTradingDeal(deal));
        }

        public ResponseContext Put(Dictionary<string, object> param)
        {
            throw new NotImplementedException();
        }

        public ResponseContext Delete(Dictionary<string, object> param)
        {
            RequestContext request = (RequestContext)param["request"];
            MtcgClient client = (MtcgClient)param["client"];

            //trade with existing deal
            if (!string.IsNullOrWhiteSpace(request.RequestedResource))
            {
                TradingDeal pendingDeal = null;
                if (!_marketController.TradingDealExists(request.RequestedResource, out pendingDeal))
                    return new ResponseContext(request, new KeyValuePair<StatusCode, object>(StatusCode.BadRequest, $"There is no open deal with the given id {request.RequestedResource}"));

                if (pendingDeal.PublisherId != client.User.UserId)
                    return new ResponseContext(request, new KeyValuePair<StatusCode, object>(StatusCode.BadRequest, "You can't delete a trade that you did not create"));

                return new ResponseContext(request, _marketController.DeleteTradingDeal(pendingDeal));
            }

            return new ResponseContext(request, new KeyValuePair<StatusCode, object>(StatusCode.BadRequest, "You must provide an id of an open trading deal"));
        }
    }
}
