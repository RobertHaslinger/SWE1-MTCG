using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SWE1_MTCG.Client
{
    public class UserStatistic
    {
        public int Elo { get; set; } = 100;
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }

        [JsonInclude, JsonPropertyName("Win Rate")]
        public string Ratio => (Wins==0 || Losses==0 || Draws==0)? "100%" : $"{Convert.ToDouble(Wins)/(Wins+Losses+Draws)*100}%";
    }
}
