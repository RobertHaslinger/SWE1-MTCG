﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Cards;

namespace SWE1_MTCG.Interfaces
{
    public interface ICardCollection
    {
        IEnumerable<Card> GetAllCards();
    }
}
