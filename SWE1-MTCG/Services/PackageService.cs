using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Database;

namespace SWE1_MTCG.Services
{
    public class PackageService : IPackageService
    {
        public bool CreatePackage(Package package)
        {
            string statement = "INSERT INTO mtcg.\"Package\"(\"PackageTypeId\", \"Name\", \"Cards\") " +
                               "VALUES(@packageTypeId, @name, @cards)";

            using (NpgsqlCommand cmd = new NpgsqlCommand(statement, PostgreSQLSingleton.GetInstance.Connection))
            {
                cmd.Parameters.Add("packageTypeId", NpgsqlDbType.Integer).Value = (int)package.PackageType;
                cmd.Parameters.Add("name", NpgsqlDbType.Varchar).Value = "WS2020 Package";
                cmd.Parameters.Add("cards", NpgsqlDbType.Varchar).Value = JsonSerializer.Serialize(package.GetAllCards()
                    .Select(c => c.Guid)
                    .ToArray());
                cmd.Prepare();
                //ExecuteNonQuery returns affected rows
                return cmd.ExecuteNonQuery() == 1;
            }
        }
    }
}
