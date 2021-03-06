﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Client;
using SWE1_MTCG.Database;
using SWE1_MTCG.Server;

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

        public bool EditStats(MtcgClient client)
        {
            string statement = "UPDATE mtcg.\"User\" " +
                               "SET \"Stats\"=@stats " +
                               "WHERE \"Username\"=@username AND \"Password_Hash\"=@password";
            string[] credentials = client.User.Credentials.Split(':', 2);
            using (NpgsqlCommand cmd = new NpgsqlCommand(statement, PostgreSQLSingleton.GetInstance.Connection))
            {
                cmd.Parameters.Add("username", NpgsqlDbType.Varchar).Value = credentials[0];
                cmd.Parameters.Add("password", NpgsqlDbType.Bytea).Value = Encoding.UTF8.GetBytes(credentials[1]);
                cmd.Parameters.Add("stats", NpgsqlDbType.Varchar).Value = JsonSerializer.Serialize(client.User.Stats);
                cmd.Prepare();
                if (cmd.ExecuteNonQuery() != 1)
                {
                    return false;
                }
            }
            return true;
        }

        public bool DeleteFromStack(int userId, Guid cardId)
        {
            string statement = "";
            User user = null;
            MtcgClient client = null;
            if ((client =
                ClientMapSingleton.GetInstance.ClientMap.Values.FirstOrDefault(c => c.User.UserId == userId)) == null)
            {
                statement = "SELECT * FROM mtcg.\"User\" " +
                            "WHERE \"Id\"=@userId";
                using (NpgsqlCommand cmd = new NpgsqlCommand(statement, PostgreSQLSingleton.GetInstance.Connection))
                {
                    cmd.Parameters.Add("userId", NpgsqlDbType.Integer).Value = userId;
                    cmd.Prepare();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.SingleResult))
                    {
                        if (reader.Read())
                        {
                            user= new User(reader);
                        }

                    }
                }

                if (user == null)
                    return false;

                user.Stack.RemoveCard(cardId);
            }
            else
            {
                client.User.Stack.RemoveCard(cardId);
                ClientMapSingleton.GetInstance.ClientMap.AddOrUpdate(client.SessionToken, client,
                    (key, oldValue) => client);
                user = client.User;
            }

            statement = "UPDATE mtcg.\"User\" " +
                               "SET \"Stack\"=@stack " +
                               "WHERE \"Id\"=@userId";
            using (NpgsqlCommand cmd = new NpgsqlCommand(statement, PostgreSQLSingleton.GetInstance.Connection))
            {
                cmd.Parameters.Add("userId", NpgsqlDbType.Integer).Value = userId;
                cmd.Parameters.Add("stack", NpgsqlDbType.Varchar).Value = JsonSerializer.Serialize(user.Stack.GetAllCards());
                cmd.Prepare();
                if (cmd.ExecuteNonQuery() != 1)
                {
                    return false;
                }
            }

            return true;
        }

        public bool AddToStack(int userId, Guid cardId)
        {
            string statement = "";
            User user = null;
            MtcgClient client = null;
            if ((client =
                ClientMapSingleton.GetInstance.ClientMap.Values.FirstOrDefault(c => c.User.UserId == userId)) == null)
            {
                statement = "SELECT * FROM mtcg.\"User\" " +
                            "WHERE \"Id\"=@userId";
                using (NpgsqlCommand cmd = new NpgsqlCommand(statement, PostgreSQLSingleton.GetInstance.Connection))
                {
                    cmd.Parameters.Add("userId", NpgsqlDbType.Integer).Value = userId;
                    cmd.Prepare();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.SingleResult))
                    {
                        if (reader.Read())
                        {
                            user = new User(reader);
                        }

                    }
                }

                if (user == null)
                    return false;

                user.Stack.AddCard(cardId);
            }
            else
            {
                client.User.Stack.AddCard(cardId);
                ClientMapSingleton.GetInstance.ClientMap.AddOrUpdate(client.SessionToken, client,
                    (key, oldValue) => client);
                user = client.User;
            }

            statement = "UPDATE mtcg.\"User\" " +
                               "SET \"Stack\"=@stack " +
                               "WHERE \"Id\"=@userId";
            using (NpgsqlCommand cmd = new NpgsqlCommand(statement, PostgreSQLSingleton.GetInstance.Connection))
            {
                cmd.Parameters.Add("userId", NpgsqlDbType.Integer).Value = userId;
                cmd.Parameters.Add("stack", NpgsqlDbType.Varchar).Value = JsonSerializer.Serialize(user.Stack.GetAllCards());
                cmd.Prepare();
                if (cmd.ExecuteNonQuery() != 1)
                {
                    return false;
                }
            }

            return true;
        }

        public List<string> GetScoreboard(MtcgClient client)
        {
            List<User> users= new List<User>();
            string statement = "SELECT * FROM mtcg.\"User\"";
            using (NpgsqlCommand cmd = new NpgsqlCommand(statement, PostgreSQLSingleton.GetInstance.Connection))
            {
                cmd.Prepare();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        User user = new User(reader);
                        users.Add(user);
                    }

                }
            }

            return users.OrderByDescending(u => u.Stats.Elo)
                .Select((u, rank) => GetRankString(client, u, rank+1)).ToList();
        }

        private string GetRankString(MtcgClient client, User user, int rank)
        {
            if (client.User.Username != user.Username)
                return $"{rank}. {user.Username}: {user.Stats.Elo} ELO";
            return $"You ({user.Username}) are in the {rank}. place {user.Stats.Elo} ELO!";
        }
    }
}
