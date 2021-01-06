using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWE1_MTCG.Client;

namespace SWE1_MTCG.Server
{
    public sealed class BattleDispatcherSingleton
    {
        private static BattleDispatcherSingleton _singleton;
        private static readonly object InstanceLock = new object();

        private BattleDispatcherSingleton()
        {
            ClientMap = new ConcurrentDictionary<string, MtcgClient>();
        }

        public static BattleDispatcherSingleton GetInstance
        {
            get
            {
                if (_singleton == null)
                {
                    lock (InstanceLock)
                    {
                        if (_singleton == null)
                        {
                            _singleton = new BattleDispatcherSingleton();
                        }
                    }
                }

                return _singleton;
            }
        }

        public ConcurrentDictionary<string, MtcgClient> ClientMap { get; }
    }
}
