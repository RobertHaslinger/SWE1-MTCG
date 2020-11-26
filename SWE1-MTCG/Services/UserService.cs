using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Database;

namespace SWE1_MTCG.Services
{
    public class UserService : IUserService
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
                using (var reader = cmd.ExecuteReader(CommandBehavior.SingleResult))
                {
                    if (reader.Read())
                    {
                        var values = new object[reader.FieldCount];
                        int instances = reader.GetValues(values);
                        if (instances > 0)
                        {
                            //double checking
                            return credentials[0] == values[1].ToString() && credentials[1] == values[2].ToString();
                        }
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

            string statement = "UPDATE mtcg.\"User\"SET \"SessionId\"=@sessionToken" +
                               "WHERE \"Username\"=@username AND \"Password_Hash\"=@password";
            string[] credentials = user.Credentials.Split(':', 2);

            using (NpgsqlCommand cmd = new NpgsqlCommand(statement, PostgreSQLSingleton.GetInstance.Connection))
            {
                cmd.Parameters.Add("sessionToken", NpgsqlDbType.Varchar).Value=sessionToken;
                cmd.Parameters.Add("username", NpgsqlDbType.Varchar).Value = credentials[0];
                cmd.Parameters.Add("password", NpgsqlDbType.Bytea).Value = Encoding.UTF8.GetBytes(credentials[1]);
                cmd.Prepare();
                //ExecuteNonQuery returns affected rows
                return cmd.ExecuteNonQuery() == 1 ?  new MtcgClient(user, sessionToken) :  null;
            }
        }

        public Package AcquirePackage()
        {
            throw new NotImplementedException();
        }

        public int GetPackagePrice()
        {
            throw new NotImplementedException();
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
