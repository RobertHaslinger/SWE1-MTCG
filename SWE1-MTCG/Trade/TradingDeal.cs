using System;
using System.Collections.Generic;
using System.Text;

namespace SWE1_MTCG.Trade
{
    public class TradingDeal
    {


        #region fields

        private User _requestor;

        #endregion

        #region properties

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
        #endregion


    }
}
