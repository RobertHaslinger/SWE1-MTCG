using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Client;

namespace SWE1_MTCG.Trade
{
    public class TradingDeal
    {


        #region fields

        private User _requestor;

        #endregion

        #region properties

        public CardStat Offer { get; private set; }
        public CardStat Request { get; private set; }
        #endregion

        #region constructor
        public TradingDeal(User requestor)
        {
            _requestor = requestor;
        }

        #endregion

        #region private methods

        #endregion

        #region public methods
        public string GetRequestor()
        {
            return _requestor.Username;
        }

        public void AddOffer(CardStat offer)
        {
            Offer = offer;
        }

        public void AddRequest(CardStat request)
        {
            Request = request;
        }

        public bool IsQualifiedDeal()
        {
            return _requestor != null && Offer != null && Request != null;
        }

        public bool CardStatMatchesRequest(CardStat cardStat)
        {
            return cardStat.Name.Contains(Request.Name) && Request.Damage <= cardStat.Damage &&
                   Request.Element == cardStat.Element;
        }
        #endregion


    }
}
