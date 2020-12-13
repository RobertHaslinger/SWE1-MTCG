using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using SWE1_MTCG.Client;
using SWE1_MTCG.Controller;
using SWE1_MTCG.Dto;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Server;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Api
{
    public class UserApi : IRestApi
    {
        private UserController _userController;

        public bool AllowAnonymous => true;

        public UserApi()
        {
            IUserService userService= new UserService();
            _userController = new UserController(userService);
        }

        public ResponseContext Get(Dictionary<string, object> param)
        {
            RequestContext request = (RequestContext)param["request"];

            if (!request.QueryParams.ContainsKey("username"))
            {
                return new ResponseContext(request, new KeyValuePair<StatusCode, object>(StatusCode.BadRequest, ""));
            }

            return new ResponseContext(request, _userController.ViewProfile(request.QueryParams["username"]));
        }

        /// <summary>
        /// Register
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ResponseContext Post(Dictionary<string, object> param)
        {
            RequestContext request = (RequestContext) param["request"];
            if (!request.Headers.ContainsKey("Content-Type") || request.Headers["Content-Type"] != "application/json")
            {
                return new ResponseContext(request, new KeyValuePair<StatusCode, object>(StatusCode.UnsupportedMediaType, ""));
            }

            UserDto userDto = JsonSerializer.Deserialize<UserDto>(request.Payload);
            if (userDto==null || string.IsNullOrWhiteSpace(userDto.Username) || string.IsNullOrWhiteSpace(userDto.Password))
            {
                return new ResponseContext(request, new KeyValuePair<StatusCode, object>(StatusCode.BadRequest, "Either username or password is empty."));
            }

            return new ResponseContext(request, _userController.Register(userDto.Username, userDto.Password));
        }

        /// <summary>
        /// Edit Profile
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public virtual ResponseContext Put(Dictionary<string, object> param)
        {
            RequestContext request = (RequestContext)param["request"];
            if (!param.ContainsKey("client"))
            {
                return new ResponseContext(request, new KeyValuePair<StatusCode, object>(StatusCode.Unauthorized, "You need to log in to use this service."));
            }
            MtcgClient client = (MtcgClient) param["client"];
            if (!request.Headers.ContainsKey("Content-Type") || request.Headers["Content-Type"] != "application/json")
            {
                return new ResponseContext(request, new KeyValuePair<StatusCode, object>(StatusCode.UnsupportedMediaType, ""));
            }

            Profile profile = JsonSerializer.Deserialize<Profile>(request.Payload);
            return new ResponseContext(request, _userController.EditProfile(ref client, profile));
        }

        public ResponseContext Delete(Dictionary<string, object> param)
        {
            throw new NotImplementedException();
        }
    }
}
