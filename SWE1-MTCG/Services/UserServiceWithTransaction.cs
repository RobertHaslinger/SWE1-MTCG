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

namespace SWE1_MTCG.Services
{
    public class UserServiceWithTransaction : UserService, IPackageTransactionService, ILoggable
    {
        public void Log(KeyValuePair<string, object> param)
        {
            throw new NotImplementedException();
        }

        public bool AcquirePackage(ref MtcgClient client, PackageType type)
        {
            Package package=null;
            PackageDto packageDto=null;
            int price=0;
            string statement = "SELECT * FROM mtcg.\"Package\" p, mtcg.\"PackageType\" pT " +
                               "WHERE pt.\"Id\"=@packageTypeId AND p.\"PackageTypeId\"=pT.\"Id\"";

            using (NpgsqlCommand cmd = new NpgsqlCommand(statement, PostgreSQLSingleton.GetInstance.Connection))
            {
                cmd.Parameters.Add("packageTypeId", NpgsqlDbType.Integer).Value = (int)type;
                cmd.Prepare();
                using (var reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        packageDto = new PackageDto()
                        {
                            CardGuids = JsonSerializer.Deserialize<List<Guid>>(reader["Cards"].ToString())
                        };
                        price = (int) reader["Price"];
                        
                    }

                };
            }
            package = packageDto?.ToObject();

            if (package == null)
                return false;

            //TODO maybe remove package but not sure

            //save old properties to fallback if necessary
            Stack<Package> oldUnopenedPackages = client.User.CurrentUnopenedPackages;

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
