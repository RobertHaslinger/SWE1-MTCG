using System;
using System.Collections.Generic;
using System.Data;
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
    public class UserService : IUserService, IDeckService
    {
        public bool IsRegistered(User user)
        {
            string statement = "SELECT * FROM mtcg.\"User\"" +
                               "WHERE \"Username\"=@username AND \"Password_Hash\"=@password";
            string[] credentials = user.Credentials.Split(':', 2);
            using (NpgsqlCommand cmd = new NpgsqlCommand(statement, PostgreSQLSingleton.GetInstance.Connection))
            {
                cmd.Parameters.Add("username", NpgsqlDbType.Varchar).Value = credentials[0];
                cmd.Parameters.Add("password", NpgsqlDbType.Bytea).Value = Encoding.UTF8.GetBytes(credentials[1]);
                cmd.Prepare();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return credentials[0] == reader["Username"].ToString() && credentials[1] == Encoding.Default.GetString((byte[])reader["Password_Hash"]);
                    }

                }
            }

            return false;
        }

        public bool Register(User user)
        {
            string statement = "INSERT INTO mtcg.\"User\"(\"Username\", \"Password_Hash\") " +
                               "VALUES(@username, @password)";
            string[] credentials = user.Credentials.Split(':', 2);

            using (NpgsqlCommand cmd = new NpgsqlCommand(statement, PostgreSQLSingleton.GetInstance.Connection))
            {
                cmd.Parameters.Add("username", NpgsqlDbType.Varchar).Value = credentials[0];
                cmd.Parameters.Add("password", NpgsqlDbType.Bytea).Value = Encoding.UTF8.GetBytes(credentials[1]);
                cmd.Prepare();
                //ExecuteNonQuery returns affected rows
                return cmd.ExecuteNonQuery() == 1;
            }
        }

        public MtcgClient Login(User user)
        {
            string sessionToken = $"{user.Username}-mtcgToken";

            string statement = "UPDATE mtcg.\"User\" SET \"CurrentToken\"=@sessionToken " +
                               "WHERE \"Username\"=@username AND \"Password_Hash\"=@password";
            string[] credentials = user.Credentials.Split(':', 2);

            using (NpgsqlCommand cmd = new NpgsqlCommand(statement, PostgreSQLSingleton.GetInstance.Connection))
            {
                cmd.Parameters.Add("sessionToken", NpgsqlDbType.Varchar).Value=sessionToken;
                cmd.Parameters.Add("username", NpgsqlDbType.Varchar).Value = credentials[0];
                cmd.Parameters.Add("password", NpgsqlDbType.Bytea).Value = Encoding.UTF8.GetBytes(credentials[1]);
                cmd.Prepare();
                //ExecuteNonQuery returns affected rows
                if (cmd.ExecuteNonQuery() != 1) return null;
            }

            statement = "SELECT * FROM mtcg.\"User\"" +
                        "WHERE \"Username\"=@username AND \"Password_Hash\"=@password";
            using (NpgsqlCommand cmd = new NpgsqlCommand(statement, PostgreSQLSingleton.GetInstance.Connection))
            {
                cmd.Parameters.Add("username", NpgsqlDbType.Varchar).Value = credentials[0];
                cmd.Parameters.Add("password", NpgsqlDbType.Bytea).Value = Encoding.UTF8.GetBytes(credentials[1]);
                cmd.Prepare();
                using (var reader = cmd.ExecuteReader(CommandBehavior.SingleResult))
                {
                    if (reader.Read())
                    {
                        return new MtcgClient(new User(reader), sessionToken);
                    }

                }
            }

            return null;
        }

        public bool ConfigureDeck(ref MtcgClient client)
        {
            string statement = "UPDATE mtcg.\"User\" " +
                        "SET \"Deck\"=@deck " +
                        "WHERE \"Username\"=@username AND \"Password_Hash\"=@password";
            string[] credentials = client.User.Credentials.Split(':', 2);
            using (NpgsqlCommand cmd = new NpgsqlCommand(statement, PostgreSQLSingleton.GetInstance.Connection))
            {
                cmd.Parameters.Add("username", NpgsqlDbType.Varchar).Value = credentials[0];
                cmd.Parameters.Add("password", NpgsqlDbType.Bytea).Value = Encoding.UTF8.GetBytes(credentials[1]);
                cmd.Parameters.Add("deck", NpgsqlDbType.Varchar).Value = JsonSerializer.Serialize(client.User.Deck.GetAllCards());
                cmd.Prepare();
                if (cmd.ExecuteNonQuery() != 1)
                {
                    return false;
                }
            }
            return true;
        }

        public Profile ViewProfile(string username)
        {
            string statement = "SELECT * FROM mtcg.\"User\"" +
                        "WHERE \"Username\"=@username";
            using (NpgsqlCommand cmd = new NpgsqlCommand(statement, PostgreSQLSingleton.GetInstance.Connection))
            {
                cmd.Parameters.Add("username", NpgsqlDbType.Varchar).Value = username;
                cmd.Prepare();
                using (var reader = cmd.ExecuteReader(CommandBehavior.SingleResult))
                {
                    if (reader.Read())
                    {
                        string profile;
                        if ((profile = reader["Profile"].ToString()) == null)
                            return null;
                        return JsonSerializer.Deserialize<Profile>(profile);
                    }

                }
            }

            return null;
        }

        public bool EditProfile(ref MtcgClient client, Profile profile)
        {
            string statement = "UPDATE mtcg.\"User\" " +
                               "SET \"Profile\"=@profile " +
                               "WHERE \"Username\"=@username AND \"Password_Hash\"=@password";
            string[] credentials = client.User.Credentials.Split(':', 2);
            using (NpgsqlCommand cmd = new NpgsqlCommand(statement, PostgreSQLSingleton.GetInstance.Connection))
            {
                cmd.Parameters.Add("username", NpgsqlDbType.Varchar).Value = credentials[0];
                cmd.Parameters.Add("password", NpgsqlDbType.Bytea).Value = Encoding.UTF8.GetBytes(credentials[1]);
                cmd.Parameters.Add("profile", NpgsqlDbType.Varchar).Value = JsonSerializer.Serialize(profile);
                cmd.Prepare();
                if (cmd.ExecuteNonQuery() != 1)
                {
                    return false;
                }
            }

            client.User.Profile = profile;
            return true;
        }
    }
}
