using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Services
{
    public class ElementServiceBase : IElementService
    {
        /// <summary>
        /// Compares two elements.
        /// </summary>
        /// <param name="element1"></param>
        /// <param name="element2"></param>
        /// <returns>double - The Effectiveness of element1 against element2.</returns>
        public double CompareElement(ElementType element1, ElementType element2)
        {
            switch (element1)
            {
                case ElementType.Fire:
                {
                    return element2 switch
                    {
                        ElementType.Normal => ElementEffectiveness.Effective,
                        ElementType.Water => ElementEffectiveness.NotEffective,
                        _ => ElementEffectiveness.Normal
                    };
                }
                case ElementType.Water:
                {
                    switch (element2)
                    {
                        case ElementType.Fire: return ElementEffectiveness.Effective;
                        case ElementType.Normal: return ElementEffectiveness.NotEffective;
                        default: return ElementEffectiveness.Normal;
                    }
                }
                case ElementType.Normal:
                {
                    return element2 switch
                    {
                        ElementType.Water => ElementEffectiveness.Effective,
                        ElementType.Fire => ElementEffectiveness.NotEffective,
                        _ => ElementEffectiveness.Normal
                    };
                }
                default: return ElementEffectiveness.Normal;
            }
        }
    }
}
