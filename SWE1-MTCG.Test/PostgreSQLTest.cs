using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using NUnit.Framework;
using SWE1_MTCG.Database;

namespace SWE1_MTCG.Test
{
    [TestFixture]
    public class PostgreSQLTest
    {
        private NpgsqlConnection _dbConnection;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _dbConnection = PostgreSQLSingleton.GetInstance.Connection;
            _dbConnection.Open();
        }

        [Test, Order(1)]
        public void Test_DbConnectionCanInsertUser()
        {
            User user= new User("testUser", "testPassword");
            string[] credentials = user.Credentials.Split(':', 2);
            int affectedRows;

            using (NpgsqlCommand insertCommand = new NpgsqlCommand(PostgreSQLCommands.InsertUserCommand, _dbConnection))
            {
                insertCommand.Parameters.Add("username", NpgsqlDbType.Varchar).Value=credentials[0];
                insertCommand.Parameters.Add("password", NpgsqlDbType.Bytea).Value= Encoding.UTF8.GetBytes(credentials[1]);
                insertCommand.Prepare();
                affectedRows =insertCommand.ExecuteNonQuery();
            }

            Assert.IsTrue(affectedRows == 1);
        }

        [Test, Order(2)]
        public void Test_DbConnectionCanReadUser()
        {
            User user = new User("testUser", "testPassword");
            User returnedUser = null;
            string[] credentials = user.Credentials.Split(':', 2);

            using (NpgsqlCommand readCommand = new NpgsqlCommand(PostgreSQLCommands.ReadUserCommand, _dbConnection))
            {
                readCommand.Parameters.Add("username", NpgsqlDbType.Varchar).Value = credentials[0];
                readCommand.Parameters.Add("password", NpgsqlDbType.Bytea).Value = Encoding.UTF8.GetBytes(credentials[1]);
                readCommand.Prepare();
                using (var reader = readCommand.ExecuteReader(CommandBehavior.SingleResult))
                {
                    if (reader.Read())
                    {
                        var values = new object[reader.FieldCount];
                        int instances = reader.GetValues(values);
                        if (instances > 0)
                        {
                            returnedUser = new User(values[1].ToString(), values[2].ToString());
                        }
                    }

                }
            }

            Assert.IsNotNull(returnedUser);
        }

        [Test, Order(3)]
        public void Test_DbConnectionCanDeleteUser()
        {
            User user = new User("testUser", "testPassword");
            string[] credentials = user.Credentials.Split(':', 2);
            int affectedRows;

            using (NpgsqlCommand deleteCommand = new NpgsqlCommand(PostgreSQLCommands.DeleteUserCommand, _dbConnection))
            {
                deleteCommand.Parameters.Add("username", NpgsqlDbType.Varchar).Value = credentials[0];
                deleteCommand.Parameters.Add("password", NpgsqlDbType.Bytea).Value = Encoding.UTF8.GetBytes(credentials[1]);
                deleteCommand.Prepare();
                affectedRows = deleteCommand.ExecuteNonQuery();
            }

            Assert.IsTrue(affectedRows == 1);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {

            _dbConnection.Close();
        }
    }
}