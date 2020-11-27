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
        bool AllowAnonymous { get; }

        ResponseContext Get(Dictionary<string, object> param);
        ResponseContext Post(Dictionary<string, object> param);
        ResponseContext Put(Dictionary<string, object> param);
        ResponseContext Delete(Dictionary<string, object> param);
    }
}
