using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Interfaces;

namespace SWE1_MTCG.Cards.Monster
{
    public class FireElve : Card, IMonster
    {
        public FireElve(string name, double damage, ElementType element)
        {
            Name = name;
            Damage = damage;
            Element = element;
        }

        public bool TryEvadeAttack(Card dragon)
        {
            return dragon is Dragon;
        }
    }
}
