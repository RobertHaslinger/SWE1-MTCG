using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.JsonConverter;

namespace SWE1_MTCG.Cards
{
    [JsonConverter(typeof(CardConverter))]
    public abstract class Card
    {
        public Guid Guid { get; set; }
        public string Name { get; protected set; }
        public double Damage { get; protected set; }
        public ElementType Element { get; protected set; }

        protected Card(string name, double damage, ElementType element)
        {
            Name = name;
            Damage = damage;
            Element = element;
        }

        public CardStat GetCardStat()
        {
            return new CardStat(Name, Damage, Element);
        }

        public override string ToString()
        {
            string type = GetType().FullName;
            string element = Enum.GetName(typeof(ElementType), Element);

            return $"Guid:{Guid}\nType:{type}\nName:{Name}\nDamage:{Damage}\nElement:{element}";
        }

        public static Card Parse(string input)
        {
            Dictionary<string, string> props= new Dictionary<string, string>();
            foreach (string property in input.Split("\n"))
            {
                if (string.IsNullOrWhiteSpace(property)) continue;

                string[] valuePair = property.Split(':');
                props.Add(valuePair[0], valuePair[1]);
            }
            Type t = Type.GetType(props["Type"]);
            if (t == null)
                return null;

            Card card;
            if (t.IsAssignableTo(typeof(ISpell)))
            {
                card = (Card) Activator.CreateInstance(t, props["Name"], Convert.ToDouble(props["Damage"]));
            }
            else
            {
                card = (Card) Activator.CreateInstance(t, props["Name"], Convert.ToDouble(props["Damage"]),
                    Enum.Parse<ElementType>(props["Element"]));
            }

            if (card!=null)
                card.Guid= System.Guid.Parse(props["Guid"]);

            return card;
        }
    }
}
