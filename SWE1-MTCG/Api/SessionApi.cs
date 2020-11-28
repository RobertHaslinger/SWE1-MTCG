using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using SWE1_MTCG.Controller;
using SWE1_MTCG.Dto;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Server;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Api
{
    public class SessionApi : IRestApi
    {
        private UserController _userController;
        public bool AllowAnonymous => true;

        public SessionApi()
        {
            IUserService userService= new UserService();
            _userController= new UserController(userService);
        }

        public ResponseContext Get(Dictionary<string, object> param)
        {
            throw new NotImplementedException();
        }

        public ResponseContext Post(Dictionary<string, object> param)
        {
            RequestContext request = (RequestContext)param["request"];
            if (!request.Headers.ContainsKey("Content-Type") || request.Headers["Content-Type"] != "application/json")
            {
                return new ResponseContext(request, new KeyValuePair<StatusCode, object>(StatusCode.UnsupportedMediaType, ""));
            }

            UserDto userDto = JsonSerializer.Deserialize<UserDto>(request.Payload);
            if (string.IsNullOrWhiteSpace(userDto.Username) || string.IsNullOrWhiteSpace(userDto.Password))
            {
                return new ResponseContext(request, new KeyValuePair<StatusCode, object>(StatusCode.BadRequest, "Either username or password is empty."));
            }
            User user= new User(userDto.Username, userDto.Password);

            return new ResponseContext(request, _userController.Login(user));
        }

        public ResponseContext Put(Dictionary<string, object> param)
        {
            throw new NotImplementedException();
        }

        public ResponseContext Delete(Dictionary<string, object> param)
        {
            throw new NotImplementedException();
        }
    }
}
