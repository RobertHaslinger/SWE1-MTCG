using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SWE1_MTCG.Cards;

namespace SWE1_MTCG.JsonConverter
{
    public class CardConverter : System.Text.Json.Serialization.JsonConverter<Card>
    {
        public override Card Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return Card.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, Card value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
