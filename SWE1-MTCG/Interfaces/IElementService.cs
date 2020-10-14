using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Interfaces
{
    public interface IElementService
    {
        double CompareElement(ElementType fire, ElementType normal);
    }
}
