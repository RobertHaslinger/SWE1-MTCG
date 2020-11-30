using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Database;

namespace SWE1_MTCG.Services
{
    public class CardService : ICardService
    {
        /// <summary>
        /// Check for Card Guid in DB.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public bool CardExists(Guid guid)
        {
            string statement= "SELECT * FROM mtcg.\"Card\" " +
                "WHERE \"Guid\"=@guid";

            using (NpgsqlCommand cmd = new NpgsqlCommand(statement, PostgreSQLSingleton.GetInstance.Connection))
            {
                cmd.Parameters.Add("guid", NpgsqlDbType.Uuid).Value = guid;
                cmd.Prepare();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return Guid.Parse(reader["Guid"].ToString() ?? string.Empty) == guid;
                    }

                }
            }

            return false;
        }

        /// <summary>
        /// Check for Card Name and Card Type in DB.
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public bool CardExists(Card card)
        {
            string statement = "SELECT * FROM mtcg.\"Card\" " +
                               "WHERE \"Name\"=@name AND \"Type\"=@type";
            using (NpgsqlCommand cmd = new NpgsqlCommand(statement, PostgreSQLSingleton.GetInstance.Connection))
            {
                cmd.Parameters.Add("name", NpgsqlDbType.Varchar).Value = card.Name;
                cmd.Parameters.Add("type", NpgsqlDbType.Varchar).Value = card.GetType().Name;
                cmd.Prepare();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return card.Name == reader["Name"].ToString() && card.GetType().Name == reader["Type"].ToString();
                    }

                }
            }

            return false;
        }

        public Card CreateCard(Card card)
        {
            Guid guid= Guid.NewGuid();
            card.Guid = guid;
            string statement = "INSERT INTO mtcg.\"Card\"(\"Guid\", \"Type\", \"Name\", \"Damage\", \"Element\") " +
                               "VALUES(@guid, @type, @name, @damage, @element)";

            using (NpgsqlCommand cmd = new NpgsqlCommand(statement, PostgreSQLSingleton.GetInstance.Connection))
            {
                cmd.Parameters.Add("guid", NpgsqlDbType.Uuid).Value = card.Guid;
                cmd.Parameters.Add("type", NpgsqlDbType.Varchar).Value = card.GetType().Name;
                cmd.Parameters.Add("name", NpgsqlDbType.Varchar).Value = card.Name;
                cmd.Parameters.Add("damage", NpgsqlDbType.Double).Value = card.Damage;
                cmd.Parameters.Add("element", NpgsqlDbType.Varchar).Value = Enum.GetName(card.Element);
                cmd.Prepare();
                //ExecuteNonQuery returns affected rows
                return cmd.ExecuteNonQuery() == 1 ? card : null;
            }
        }

        public Card DeleteCard(Guid guid)
        {
            throw new NotImplementedException();
        }
    }
}
