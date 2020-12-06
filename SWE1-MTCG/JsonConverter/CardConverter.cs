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
            //StartObject
            reader.Read();
            reader.Read();
            //GuidValue
            string guid= reader.GetString();
            reader.Read();
            reader.Read();
            //TypeValue
            string type = reader.GetString();
            reader.Read();
            reader.Read();
            //NameValue
            string name = reader.GetString();
            reader.Read();
            reader.Read();
            //DamageValue
            string damage = reader.GetString();
            reader.Read();
            reader.Read();
            //ElementValue
            string element = reader.GetString();
            //EndObject
            reader.Read();
            return Card.Parse(guid, type, name, damage, element);
        }

        public override void Write(Utf8JsonWriter writer, Card value, JsonSerializerOptions options)
        {
            Dictionary<string, string> props = new Dictionary<string, string>();
            
            writer.WriteStartObject();
            foreach (string property in value.ToString().Split("\r\n"))
            {
                if (string.IsNullOrWhiteSpace(property)) continue;

                string[] valuePair = property.Split(':');
                writer.WriteString(valuePair[0], valuePair[1]);
            }
            writer.WriteEndObject();
        }
    }
}
