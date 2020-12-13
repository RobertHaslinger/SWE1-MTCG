using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWE1_MTCG.Client;
using SWE1_MTCG.Database;

namespace SWE1_MTCG.Server
{
    public sealed class ClientMapSingleton
    {
        private static ClientMapSingleton _singleton;
        private static readonly object InstanceLock = new object();

        private ClientMapSingleton()
        {
            ClientMap= new ConcurrentDictionary<string, MtcgClient>();
        }

        public static ClientMapSingleton GetInstance
        {
            get
            {
                if (_singleton == null)
                {
                    lock (InstanceLock)
                    {
                        if (_singleton == null)
                        {
                            _singleton = new ClientMapSingleton();
                        }
                    }
                }

                return _singleton;
            }
        }

        public ConcurrentDictionary<string, MtcgClient> ClientMap { get; }
    }
}
