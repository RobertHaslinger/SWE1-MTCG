using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1_MTCG.Services
{
    public interface ILoggable
    {
        void Log(KeyValuePair<string, object> param);
    }
}
