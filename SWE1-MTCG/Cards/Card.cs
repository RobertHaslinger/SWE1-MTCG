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

        public override string ToString()
        {
            string type = GetType().FullName;
            string element = Enum.GetName(typeof(ElementType), Element);

            return $"Guid:{Guid}\r\nType:{type}\r\nName:{Name}\r\nDamage:{Damage}\r\nElement:{element}";
        }

        public static Card Parse(string guid, string type, string name, string damage, string element)
        {
            Type t = Type.GetType(type);
            if (t == null)
                return null;

            Card card;
            if (t.IsAssignableTo(typeof(ISpell)))
            {
                card = (Card) Activator.CreateInstance(t, name, Convert.ToDouble(damage));
            }
            else
            {
                card = (Card) Activator.CreateInstance(t, name, Convert.ToDouble(damage),
                    Enum.Parse<ElementType>(element));
            }

            if (card!=null)
                card.Guid= Guid.Parse(guid);

            return card;
        }
    }
}
