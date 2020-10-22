using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Interfaces
{
    /// <summary>
    /// All return values are serialized objects
    /// </summary>
    public interface IRestApi
    {
        string Get(object param);
        string Post(object param);
        string Put(object param);
        string Delete(object param);
    }
}
