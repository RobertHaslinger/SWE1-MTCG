using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Dto
{
    public class PackageTypeDto : Dto<PackageType>
    {
        public string PackageType { get; set; }
        public override PackageType ToObject()
        {
            PackageType type;
            try
            {
                type = Enum.Parse<PackageType>(PackageType);
            }
            catch (Exception)
            {
                type = Enums.PackageType.Basic;
            }

            return type;
        }
    }
}
