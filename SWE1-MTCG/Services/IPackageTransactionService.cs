using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Client;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Services
{
    public interface IPackageTransactionService
    {
        bool AcquirePackage(ref MtcgClient client, PackageType type);
        int GetPackagePrice(PackageType type);
    }
}
