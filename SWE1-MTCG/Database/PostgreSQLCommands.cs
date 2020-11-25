using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace SWE1_MTCG.Database
{
    public static class PostgreSQLCommands
    {
        /// <summary>
        /// Parameters: @username, @password
        /// </summary>
        public static string InsertUserCommand => "INSERT INTO mtcg.\"User\"(\"Username\", \"Password_Hash\") " +
                                                  "VALUES(@username, @password)";

        /// <summary>
        /// Parameters: @username, @password
        /// </summary>
        public static string ReadUserCommand => "SELECT * FROM mtcg.\"User\"" +
                                                "WHERE \"Username\"=@username AND \"Password_Hash\"=@password";

        public static string DeleteUserCommand => "DELETE FROM mtcg.\"User\"" +
                                                  "WHERE \"Username\"=@username AND \"Password_Hash\"=@password";
    }
}
