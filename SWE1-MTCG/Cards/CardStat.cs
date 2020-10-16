using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Cards
{
    public class CardStat
    {
        public readonly string Name;
        public readonly double Damage;
        public readonly ElementType Element;

        public CardStat(string name, double damage, ElementType element)
        {
            Name = name;
            Damage = damage;
            Element = element;
        }
    }
}
