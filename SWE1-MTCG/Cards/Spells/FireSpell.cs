using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Interfaces;

namespace SWE1_MTCG.Cards.Spells
{
    public class FireSpell : Card, ISpell
    {
        public FireSpell(string name, double damage) : base(name, damage, ElementType.Fire)
        {
        }
    }
}
