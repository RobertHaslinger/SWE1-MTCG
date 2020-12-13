using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Client;
using SWE1_MTCG.Database;

namespace SWE1_MTCG.Services
{
    public class UserServiceWithPackageOpen : UserService, IPackageOpenService
    {
        public bool OpenPackage(ref MtcgClient client, Package package)
        {
            client.User.Stack.AddCards(package.GetAllCards());
            string statement = "UPDATE mtcg.\"User\" " +
                        "SET \"UnopenedPackages\"=@unopenedPackages, \"Stack\"=@stack " +
                        "WHERE \"Username\"=@username AND \"Password_Hash\"=@password";
            string[] credentials = client.User.Credentials.Split(':', 2);
            using (NpgsqlCommand cmd = new NpgsqlCommand(statement, PostgreSQLSingleton.GetInstance.Connection))
            {
                cmd.Parameters.Add("username", NpgsqlDbType.Varchar).Value = credentials[0];
                cmd.Parameters.Add("password", NpgsqlDbType.Bytea).Value = Encoding.UTF8.GetBytes(credentials[1]);
                cmd.Parameters.Add("stack", NpgsqlDbType.Varchar).Value = JsonSerializer.Serialize(client.User.Stack.GetAllCards());
                cmd.Parameters.Add("unopenedPackages", NpgsqlDbType.Varchar).Value = JsonSerializer.Serialize(client.User.CurrentUnopenedPackages);
                cmd.Prepare();
                if (cmd.ExecuteNonQuery() != 1)
                {
                    foreach (Card card in package.GetAllCards())
                    {
                        client.User.Stack.RemoveCard(card);
                    }
                    client.User.CurrentUnopenedPackages.Push(package);
                    return false;
                }
            }

            return true;
        }
    }
}
