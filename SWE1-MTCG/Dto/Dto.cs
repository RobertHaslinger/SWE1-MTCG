using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1_MTCG.Dto
{
    public abstract class Dto<T>
    {
        public abstract T ToObject();
    }
}
