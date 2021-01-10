using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Client;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Services;
using SWE1_MTCG.Trade;

namespace SWE1_MTCG.Controller
{
    public class MarketController : ControllerWithDbAccess
    {

        #region fields

        private IMarketService _marketService;
        private UserService _userService;

        #endregion

        #region properties

        #endregion

        #region constructor

        public MarketController(IMarketService marketService)
        {
            _marketService = marketService;
            _userService= new UserService();
        }

        #endregion

        #region private methods

        #endregion

        #region public methods

        public KeyValuePair<StatusCode, object> GetAllOpenTradingDeals()
        {
            try
            {
                var deals = _marketService.GetAllOpenTradingDeals() as List<TradingDeal>;
                if (deals.Count==0)
                    return new KeyValuePair<StatusCode, object>(StatusCode.NoContent, "There are currently no open trading deals");

                return new KeyValuePair<StatusCode, object>(StatusCode.OK, deals);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        public KeyValuePair<StatusCode, object> GetOpenTradingDealsForUser(string username)
        {
            try
            {
                var deals = _marketService.GetOpenTradingDealsForUser(username) as List<TradingDeal>;
                if (deals.Count == 0)
                    return new KeyValuePair<StatusCode, object>(StatusCode.NoContent, $"There are currently no open trading deals for {username}");

                return new KeyValuePair<StatusCode, object>(StatusCode.OK, deals);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        public KeyValuePair<StatusCode, object> AddTradingDeal(TradingDeal dealToAdd)
        {
            try
            {
                var deal = _marketService.AddTradingDeal(dealToAdd);
                if (deal==null)
                    return new KeyValuePair<StatusCode, object>(StatusCode.InternalServerError, "Deal could not be saved");
                return new KeyValuePair<StatusCode, object>(StatusCode.Created, deal);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        public KeyValuePair<StatusCode, object> ProcessTrade(TradingDeal deal, Card cardToTrade, MtcgClient client)
        {
            try
            {
                if (deal.MinimumDamage > cardToTrade.Damage || cardToTrade.GetType().Name != deal.RequestedType ||
                    Enum.GetName(typeof(ElementType), cardToTrade.Element) != deal.RequestedElement)
                    return new KeyValuePair<StatusCode, object>(StatusCode.BadRequest,
                        $"The given card does not match the requirements Min Damage:{deal.MinimumDamage} Element: {deal.RequestedElement} Type: {deal.RequestedType}");

                _userService.AddToStack(deal.PublisherId, cardToTrade.Guid);
                _userService.AddToStack(client.User.UserId, deal.CardId);
                _userService.DeleteFromStack(deal.PublisherId, deal.Guid);
                _userService.DeleteFromStack(client.User.UserId, cardToTrade.Guid);

                _marketService.Trade(deal, client.User.UserId);
                return new KeyValuePair<StatusCode, object>(StatusCode.OK, "Trade completed successfully");
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        public bool TradingDealExists(string dealId, out TradingDeal deal)
        {
            try
            {
                Guid guid= Guid.Parse(dealId);
                return (deal = _marketService.GetOpenTradingDeal(guid)) != null;
            }
            catch (Exception e)
            {
                deal = null;
                return false;
            }
        }

        public KeyValuePair<StatusCode, object> DeleteTradingDeal(TradingDeal deal)
        {
            try
            {
                if (_marketService.DeleteTradingDeal(deal))
                    return new KeyValuePair<StatusCode, object>(StatusCode.OK, $"Deleted {deal.Guid} successfully");

                return new KeyValuePair<StatusCode, object>(StatusCode.InternalServerError, "Something went wrong");
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }
        #endregion

    }
}
