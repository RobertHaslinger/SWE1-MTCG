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
                    switch (element2)
                    {
                            case ElementType.Normal: return ElementEffectiveness.Effective;
                            case ElementType.Water: return ElementEffectiveness.NotEffective;
                            default: return ElementEffectiveness.Normal;
                    }
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
                default:
                {
                    switch (element2)
                    {
                        case ElementType.Water: return ElementEffectiveness.Effective;
                        case ElementType.Fire: return ElementEffectiveness.NotEffective;
                        default: return ElementEffectiveness.Normal;
                    }
                }
            }
        }
    }
}
