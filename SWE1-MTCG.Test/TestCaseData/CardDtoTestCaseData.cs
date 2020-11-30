using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWE1_MTCG.Cards.Monster;
using SWE1_MTCG.Cards.Spells;
using SWE1_MTCG.Dto;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Test.TestCaseData
{
    public class CardDtoTestCaseData
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return new NUnit.Framework.TestCaseData(new CardDto()
                {
                    CardType = "Dragon",
                    Damage = 45.3,
                    Element = "Fire",
                    Name = "Great Fire Dragon"
                }, new Dragon("Great Fire Dragon", 45.3, ElementType.Fire));
                yield return new NUnit.Framework.TestCaseData(new CardDto()
                {
                    CardType = "Typo",
                    Damage = 1000,
                    Element = "Normal",
                    Name = "Stan Lee"
                }, null);
                yield return new NUnit.Framework.TestCaseData(new CardDto()
                {
                    CardType = "WaterSpell",
                    Damage = 27,
                    Name = "Cascade"
                }, new WaterSpell("Cascade", 27));
            }
        }
    }
}
