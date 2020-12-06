using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Controller;
using SWE1_MTCG.Dto;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Server;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Api
{
    public class PackageApi : IRestApi
    {
        private PackageController _packageController;
        public virtual bool AllowAnonymous => true;

        public PackageApi()
        {
            IPackageService packageService= new PackageService();
            _packageController= new PackageController(packageService);
        }

        public virtual ResponseContext Get(Dictionary<string, object> param)
        {
            throw new NotImplementedException();
        }

        public virtual ResponseContext Post(Dictionary<string, object> param)
        {
            RequestContext request = (RequestContext)param["request"];
            if (!request.Headers.ContainsKey("Content-Type") || request.Headers["Content-Type"] != "application/json")
            {
                return new ResponseContext(request, new KeyValuePair<StatusCode, object>(StatusCode.UnsupportedMediaType, ""));
            }

            PackageDto packageDto = JsonSerializer.Deserialize<PackageDto>(request.Payload);
            Package package;
            if (packageDto == null || packageDto.CardGuids.Count< 5 ||(package = packageDto.ToObject()) == null)
            {
                return new ResponseContext(request,
                    new KeyValuePair<StatusCode, object>(StatusCode.BadRequest,
                        "Either the package does not contain 5 cards or some cards do not exist"));
            }

            return new ResponseContext(request, _packageController.CreatePackage(package));
        }

        public virtual ResponseContext Put(Dictionary<string, object> param)
        {
            throw new NotImplementedException();
        }

        public virtual ResponseContext Delete(Dictionary<string, object> param)
        {
            throw new NotImplementedException();
        }
    }
}
