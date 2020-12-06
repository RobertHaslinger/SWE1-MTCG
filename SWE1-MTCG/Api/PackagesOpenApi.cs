using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SWE1_MTCG.Controller;
using SWE1_MTCG.Dto;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Server;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Api
{
    public class PackagesOpenApi : PackageApi
    {
        private UserController _userController;
        public override bool AllowAnonymous => false;

        public PackagesOpenApi()
        {
            IUserService userService= new UserServiceWithPackageOpen();
            _userController= new UserController(userService);
        }

        public override ResponseContext Post(Dictionary<string, object> param)
        {
            RequestContext request = (RequestContext)param["request"];
            MtcgClient client = (MtcgClient)param["client"];

            return new ResponseContext(request, _userController.OpenPackage(ref client));
        }
    }
}
