using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Interfaces;

namespace SWE1_MTCG.Cards.Monster
{
    public class Dragon : Card, IMonster
    {
        public Dragon(string name, double damage, ElementType element)
        {
            Name = name;
            Damage = damage;
            Element = element;
        }
    }
}
