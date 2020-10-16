using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Interfaces;

namespace SWE1_MTCG.Cards.Monster
{
    public class FireElf : Card, IMonster
    {
        public FireElf(string name, double damage, ElementType element) : base(name, damage, element)
        {
        }

        public bool TryEvadeAttack(Card dragon)
        {
            return dragon is Dragon;
        }
    }
}
