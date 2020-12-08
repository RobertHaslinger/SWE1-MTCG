using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Database;
using SWE1_MTCG.Dto;
using SWE1_MTCG.Enums;
using SWE1_MTCG.JsonConverter;

namespace SWE1_MTCG.Services
{
    public class UserServiceWithTransaction : UserService, IPackageTransactionService, ILoggable
    {

        private List<Guid> AcquireRandomPackage()
        {
            List<Guid> guids= new List<Guid>();
            string statement = "SELECT * FROM mtcg.\"Card\" \r\nORDER BY RANDOM()\r\nLIMIT 5;";
            using (NpgsqlCommand cmd = new NpgsqlCommand(statement, PostgreSQLSingleton.GetInstance.Connection))
            {
                cmd.Prepare();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        guids.Add(Guid.Parse(reader["Guid"].ToString()));
                    }

                };
            }

            return guids;
        }

        public bool Log(Dictionary<string, object> param)
        {
            string statement = "INSERT INTO mtcg.\"Transactions\"(\"UserId\", \"Description\", \"Date\") " +
                               "VALUES(@userId, @description, @date)";

            using (NpgsqlCommand cmd = new NpgsqlCommand(statement, PostgreSQLSingleton.GetInstance.Connection))
            {
                cmd.Parameters.Add("userId", NpgsqlDbType.Integer).Value = ((MtcgClient)param["client"]).User.UserId;
                cmd.Parameters.Add("description", NpgsqlDbType.Varchar).Value = param["description"].ToString();
                cmd.Parameters.Add("date", NpgsqlDbType.Date).Value = DateTime.UtcNow;
                cmd.Prepare();
                //ExecuteNonQuery returns affected rows
                return cmd.ExecuteNonQuery() == 1;
            }
        }

        public bool AcquirePackage(ref MtcgClient client, PackageType type)
        {
            Package package=null;
            CardCollectionDto packageDto=null;
            int price=0;
            string statement;
            if (type == PackageType.Random)
            {
                packageDto= new CardCollectionDto()
                {
                    CardGuids = AcquireRandomPackage(),
                    CardCollectionType = typeof(Package)
                };
                price = GetPackagePrice(PackageType.Random);
            }
            else
            {
                statement = "SELECT * FROM mtcg.\"Package\" p, mtcg.\"PackageType\" pT " +
                                   "WHERE pt.\"Id\"=@packageTypeId AND p.\"PackageTypeId\"=pT.\"Id\" " +
                                   "ORDER BY RANDOM() " +
                                   "LIMIT 1;";

                using (NpgsqlCommand cmd = new NpgsqlCommand(statement, PostgreSQLSingleton.GetInstance.Connection))
                {
                    cmd.Parameters.Add("packageTypeId", NpgsqlDbType.Integer).Value = (int)type;
                    cmd.Prepare();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            packageDto = new CardCollectionDto()
                            {
                                CardGuids = JsonSerializer.Deserialize<List<Guid>>(reader["Cards"].ToString()),
                                CardCollectionType = typeof(Package)
                            };
                            price = (int)reader["Price"];

                        }

                    };
                }
            }
            
            package = packageDto?.ToObject() as Package;

            if (package == null)
                return false;

            package.PackageType = type;

            //save old properties to fallback if necessary
            Stack<Package> oldUnopenedPackages = new Stack<Package>(new Stack<Package>(client.User.CurrentUnopenedPackages));

            client.User.RemoveCoins(price);
            client.User.AddPackage(package);

            statement = "UPDATE mtcg.\"User\" " +
                        "SET \"Coins\"=@coins, \"UnopenedPackages\"=@unopenedPackages " +
                        "WHERE \"Username\"=@username AND \"Password_Hash\"=@password";
            string[] credentials = client.User.Credentials.Split(':', 2);
            using (NpgsqlCommand cmd = new NpgsqlCommand(statement, PostgreSQLSingleton.GetInstance.Connection))
            {
                cmd.Parameters.Add("username", NpgsqlDbType.Varchar).Value = credentials[0];
                cmd.Parameters.Add("password", NpgsqlDbType.Bytea).Value = Encoding.UTF8.GetBytes(credentials[1]);
                cmd.Parameters.Add("coins", NpgsqlDbType.Integer).Value = client.User.Coins;
                cmd.Parameters.Add("unopenedPackages", NpgsqlDbType.Varchar).Value = JsonSerializer.Serialize(client.User.CurrentUnopenedPackages);
                cmd.Prepare();
                if (cmd.ExecuteNonQuery() != 1)
                {
                    client.User.AddCoins(price);
                    client.User.CurrentUnopenedPackages = oldUnopenedPackages;
                    return false;
                }
            }

            return true;
        }

        public int GetPackagePrice(PackageType type)
        {
            string statement = "SELECT * FROM mtcg.\"PackageType\"" +
                               "WHERE \"Id\"=@packageTypeId";
            using (NpgsqlCommand cmd = new NpgsqlCommand(statement, PostgreSQLSingleton.GetInstance.Connection))
            {
                cmd.Parameters.Add("packageTypeId", NpgsqlDbType.Integer).Value = (int)type;
                cmd.Prepare();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return (int)reader["Price"];
                    }

                }
            }

            return -1;
        }

        public Task QueueForBattle()
        {
            throw new NotImplementedException();
        }

        public void CancelQueue()
        {
            throw new NotImplementedException();
        }
    }
}
