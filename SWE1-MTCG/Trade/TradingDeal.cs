using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Npgsql;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Client;

namespace SWE1_MTCG.Trade
{
    public class TradingDeal
    {


        #region fields

        #endregion

        #region properties

        public Guid Guid { get; set; }
        public int PublisherId { get; set; }
        public int FullfillerId { get; set; }
        public bool IsFullfilled { get; set; }
        public Guid CardId { get; set; }
        public string RequestedType { get; set; }
        public double MinimumDamage { get; set; }
        public string RequestedElement { get; set; }
        #endregion

        #region constructor

        public TradingDeal()
        {
            
        }

        public TradingDeal(NpgsqlDataReader reader)
        {
            Guid= System.Guid.Parse(reader["Guid"].ToString());
            PublisherId = (int)reader["PublisherId"];
            FullfillerId = string.IsNullOrWhiteSpace(reader["FullfillerId"].ToString()) ? -1 : (int) reader["FullfillerId"];
            IsFullfilled = (bool) reader["IsFullfilled"];
            CardId= System.Guid.Parse(reader["CardId"].ToString());
            RequestedType = reader["RequestedType"].ToString();
            MinimumDamage = (double) reader["MinimumDamage"];
            RequestedElement = reader["RequestedElement"].ToString();
        }

        #endregion

        #region private methods

        #endregion

        #region public methods
        #endregion


    }
}
