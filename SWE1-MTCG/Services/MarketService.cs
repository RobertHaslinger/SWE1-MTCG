using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.Json;
using Npgsql;
using NpgsqlTypes;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Client;
using SWE1_MTCG.Database;
using SWE1_MTCG.Trade;

namespace SWE1_MTCG.Services
{
    public class MarketService : IMarketService
    {
        public IEnumerable GetOpenTradingDealsForUser(string username)
        {
            List<TradingDeal> deals= new List<TradingDeal>();
            string statement = "SELECT * FROM mtcg.\"User\" u, mtcg.\"TradingDeal\" td " +
                               "WHERE u.\"Username\"=@username AND u.\"Id\"=td.\"PublisherId\" AND \"IsFullfilled\"=FALSE";
            using (NpgsqlCommand cmd = new NpgsqlCommand(statement, PostgreSQLSingleton.GetInstance.Connection))
            {
                cmd.Parameters.Add("username", NpgsqlDbType.Varchar).Value = username;
                cmd.Prepare();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        deals.Add(new TradingDeal(reader));
                    }
                }
            }

            return deals;
        }

        public IEnumerable GetAllOpenTradingDeals()
        {
            List<TradingDeal> deals = new List<TradingDeal>();
            string statement = "SELECT * FROM mtcg.\"TradingDeal\" " +
                               "WHERE \"IsFullfilled\"=FALSE";
            using (NpgsqlCommand cmd = new NpgsqlCommand(statement, PostgreSQLSingleton.GetInstance.Connection))
            {
                cmd.Prepare();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        deals.Add(new TradingDeal(reader));
                    }
                }
            }

            return deals;
        }

        public TradingDeal AddTradingDeal(TradingDeal deal)
        {
            Guid guid = Guid.NewGuid();
            deal.Guid = guid;
            string statement =
                "INSERT INTO mtcg.\"TradingDeal\"(\"Guid\", \"PublisherId\", \"CardId\", \"RequestedType\", \"MinimumDamage\", \"RequestedElement\", \"IsFullfilled\") " +
                "VALUES (@guid, @publisherId, @cardId, @requestedType, @minimumDamage, @requestedElement, @isFullfilled);";

            using (NpgsqlCommand cmd = new NpgsqlCommand(statement, PostgreSQLSingleton.GetInstance.Connection))
            {
                cmd.Parameters.Add("guid", NpgsqlDbType.Uuid).Value = deal.Guid;
                cmd.Parameters.Add("publisherId", NpgsqlDbType.Integer).Value = deal.PublisherId;
                cmd.Parameters.Add("cardId", NpgsqlDbType.Uuid).Value = deal.CardId;
                cmd.Parameters.Add("requestedType", NpgsqlDbType.Varchar).Value = deal.RequestedType;
                cmd.Parameters.Add("minimumDamage", NpgsqlDbType.Double).Value = deal.MinimumDamage;
                cmd.Parameters.Add("requestedElement", NpgsqlDbType.Varchar).Value = deal.RequestedElement;
                cmd.Parameters.Add("isFullfilled", NpgsqlDbType.Boolean).Value = false;
                cmd.Prepare();
                //ExecuteNonQuery returns affected rows
                return cmd.ExecuteNonQuery() == 1 ? deal : null;
            }
        }

        public bool Trade(TradingDeal deal, int fullfillerId)
        {
            string statement = "UPDATE mtcg.\"TradingDeal\" " +
                               "SET \"IsFullfilled\"=TRUE, \"FullfillerId\"=@fullfillerId " +
                               "WHERE \"Guid\"=@guid";
            using (NpgsqlCommand cmd = new NpgsqlCommand(statement, PostgreSQLSingleton.GetInstance.Connection))
            {
                cmd.Parameters.Add("guid", NpgsqlDbType.Uuid).Value = deal.Guid;
                cmd.Parameters.Add("fullfillerId", NpgsqlDbType.Integer).Value = fullfillerId;
                cmd.Prepare();
                if (cmd.ExecuteNonQuery() != 1)
                {
                    return false;
                }
            }
            return true;
        }

        public TradingDeal GetOpenTradingDeal(Guid tradeId)
        {
            string statement = "SELECT * FROM mtcg.\"TradingDeal\" " +
                               "WHERE \"IsFullfilled\"=FALSE AND \"Guid\"=@guid";
            using (NpgsqlCommand cmd = new NpgsqlCommand(statement, PostgreSQLSingleton.GetInstance.Connection))
            {
                cmd.Parameters.Add("guid", NpgsqlDbType.Uuid).Value = tradeId;
                cmd.Prepare();
                using (var reader = cmd.ExecuteReader(CommandBehavior.SingleResult))
                {
                    if (reader.Read())
                        return new TradingDeal(reader);
                }
            }

            return null;
        }

        public bool DeleteTradingDeal(TradingDeal deal)
        {
            string statement = "DELETE FROM mtcg.\"TradingDeal\" " +
                               "WHERE \"Guid\"=@guid";
            using (NpgsqlCommand cmd = new NpgsqlCommand(statement, PostgreSQLSingleton.GetInstance.Connection))
            {
                cmd.Parameters.Add("guid", NpgsqlDbType.Uuid).Value = deal.Guid;
                cmd.Prepare();
                if (cmd.ExecuteNonQuery() != 1)
                {
                    return false;
                }

                return true;
            }
        }
    }
}
