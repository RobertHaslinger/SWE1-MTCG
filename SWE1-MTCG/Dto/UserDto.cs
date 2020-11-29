using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1_MTCG.Dto
{
    public class UserDto : Dto<User>
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public override User ToObject()
        {
            return new User(Username, Password);
        }
    }
}
