using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Interfaces;

namespace SWE1_MTCG.Cards.Monster
{
    public class Kraken : Card, IMonster
    {
        public Kraken(string name, double damage, ElementType element)
        {
            Name = name;
            Damage = damage;
            Element = element;
        }

        public bool TryResistSpell(Card spell)
        {
            return spell is ISpell;
        }
    }
}
