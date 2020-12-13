using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Client;
using SWE1_MTCG.Trade;

namespace SWE1_MTCG.Services
{
    public interface IMarketService
    {
        IEnumerable GetOpenTradingDealsForUser(User user);
        IEnumerable GetAllOpenTradingDeals();
        bool AddTradingDeal(TradingDeal deal);

        bool Trade(TradingDeal deal, CardStat bid);
    }
}
