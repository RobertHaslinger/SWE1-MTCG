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
    public class TransactionPackagesApi : TransactionApi
    {
        private UserController _userController;

        public TransactionPackagesApi()
        {
            IUserService userService = new UserServiceWithTransaction();
            _userController = new UserController(userService);
        }

        public override ResponseContext Post(Dictionary<string, object> param)
        {
            RequestContext request = (RequestContext)param["request"];
            MtcgClient client = (MtcgClient)param["client"];
            PackageTypeDto packageTypeDto = null;
            if (request.Payload.Length > 0)
            {
                if (!request.Headers.ContainsKey("Content-Type") ||
                    request.Headers["Content-Type"] != "application/json")
                {
                    return new ResponseContext(request,
                        new KeyValuePair<StatusCode, object>(StatusCode.UnsupportedMediaType, ""));
                }

                packageTypeDto = JsonSerializer.Deserialize<PackageTypeDto>(request.Payload);
            }

            PackageType packageType = packageTypeDto?.ToObject() ?? PackageType.Basic;
            return new ResponseContext(request, _userController.AcquirePackage(ref client, packageType));
        }
    }
}
