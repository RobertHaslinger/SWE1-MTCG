﻿using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Interfaces;

namespace SWE1_MTCG.Cards.Monster
{
    public class Goblin : Card, IMonster
    {
        public Goblin(string name, double damage, ElementType element) : base(name, damage, element)
        {
        }

        public bool TryActScared(Card possibleDragon)
        {
            return possibleDragon is Dragon;
        }
    }
}
