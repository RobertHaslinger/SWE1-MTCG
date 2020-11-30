using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Controller
{
    public class PackageController : ControllerWithDbAccess
    {
        private IPackageService _packageService;

        public PackageController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        public KeyValuePair<StatusCode, object> CreatePackage(Package package)
        {
            try
            {
                _packageService.CreatePackage(package);
                return new KeyValuePair<StatusCode, object>(StatusCode.Created, package);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }
    }
}
