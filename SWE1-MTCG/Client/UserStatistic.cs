using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using SWE1_MTCG.Battle;

namespace SWE1_MTCG.Client
{
    public class UserStatistic
    {
        public int Elo { get; set; } = 1000;
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }

        public List<BattleLog> Logs { get; set; } = new List<BattleLog>();

        [JsonInclude, JsonPropertyName("Win Rate")]
        public string Ratio => (Wins==0 && Losses==0 && Draws==0)? "Not played yet" : $"{Convert.ToDouble(Wins)/(Wins+Losses+Draws)*100}%";
    }
}
