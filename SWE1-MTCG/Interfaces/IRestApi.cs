using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Server;

namespace SWE1_MTCG.Interfaces
{
    /// <summary>
    /// All return values are serialized objects
    /// </summary>
    public interface IRestApi
    {
        ResponseContext Get(object param);
        ResponseContext Post(object param);
        ResponseContext Put(object param);
        ResponseContext Delete(object param);
    }
}
