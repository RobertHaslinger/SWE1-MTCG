﻿using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Interfaces;

namespace SWE1_MTCG.Cards.Monster
{
    public class Orc : Card, IMonster
    {
        public Orc(string name, double damage, ElementType element) : base(name, damage, element)
        {
        }
    }
}
