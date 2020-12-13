using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Client;
using SWE1_MTCG.Trade;

namespace SWE1_MTCG.Services
{
    public class MarketService : IMarketService
    {
        public IEnumerable GetOpenTradingDealsForUser(User user)
        {
            throw new NotImplementedException();
        }

        public IEnumerable GetAllOpenTradingDeals()
        {
            throw new NotImplementedException();
        }

        public bool AddTradingDeal(TradingDeal deal)
        {
            throw new NotImplementedException();
        }

        public bool Trade(TradingDeal deal, CardStat bid)
        {
            throw new NotImplementedException();
        }
    }
}
