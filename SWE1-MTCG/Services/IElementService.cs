using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Services
{
    public interface IElementService
    {
        double CompareElement(ElementType element1, ElementType element2);
    }
}
