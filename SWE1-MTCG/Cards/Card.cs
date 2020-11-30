using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Cards
{
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
    }
}
