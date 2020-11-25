using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace SWE1_MTCG.Database
{
    public sealed class PostgreSQLSingleton
    {
        private const string ConnectionString = "Host=localhost;Username=swe1_mtcg;Password=admin;Database=mtcg";
        private static PostgreSQLSingleton _connectionInstance = null;
        private static readonly object InstanceLock= new object();

        private PostgreSQLSingleton()
        {
            Connection = new NpgsqlConnection(ConnectionString);
        }

        public static PostgreSQLSingleton GetInstance
        {
            get
            {
                if (_connectionInstance == null)
                {
                    lock (InstanceLock)
                    {
                        if (_connectionInstance == null)
                        {
                            _connectionInstance = new PostgreSQLSingleton();
                        }
                    }
                }

                return _connectionInstance;
            }
        }

        public NpgsqlConnection Connection { get; }
    }
}
