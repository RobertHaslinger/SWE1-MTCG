using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Services;
using SWE1_MTCG.Trade;

namespace SWE1_MTCG.Controller
{
    public class MarketController
    {

        #region fields

        private IMarketService _marketService;

        #endregion

        #region properties

        #endregion

        #region constructor

        public MarketController(IMarketService marketService)
        {
            _marketService = marketService;
        }

        #endregion

        #region private methods

        #endregion

        #region public methods

        public IEnumerable GetAllOpenTradingDeals()
        {
            return _marketService.GetAllOpenTradingDeals();
        }

        public IEnumerable GetOpenTradingDealsForUser(User user)
        {
            return _marketService.GetOpenTradingDealsForUser(user);
        }

        public bool AddTradingDeal(TradingDeal dealToAdd)
        {
            if (!dealToAdd.IsQualifiedDeal()) return false;

            return _marketService.AddTradingDeal(dealToAdd);
        }

        public bool ProcessTrade(TradingDeal pendingDeal, CardStat bid)
        {
            if (!pendingDeal.CardStatMatchesRequest(bid)) return false;
            return _marketService.Trade(pendingDeal, bid);
        }
        #endregion

    }
}
