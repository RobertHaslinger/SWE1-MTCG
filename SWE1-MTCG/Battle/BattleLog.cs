using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWE1_MTCG.Client;

namespace SWE1_MTCG.Battle
{
    public class BattleLog
    {
        public DateTime Date= DateTime.Now;
        public string Winner { get; set; }
        public string Loser { get; set; }
        public bool Draw { get; set; }
        public List<string> Rounds { get; set; } = new List<string>();

    }
}
