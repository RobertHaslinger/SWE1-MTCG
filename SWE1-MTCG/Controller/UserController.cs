using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Npgsql;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Server;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Controller
{
    public class UserController : ControllerWithDbAccess
    {

        #region fields

        private IUserService _userService;

        #endregion

        #region constructor

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        #region private methods

        private bool HasEnoughCoins(int coins, int price)
        {
            return coins >= price;
        }
        #endregion

        #region public methods

        public KeyValuePair<StatusCode, object> Register(string username, string password)
        {
            User user = new User(username, password);
            try
            {
                _userService.Register(user);
                return new KeyValuePair<StatusCode, object>(StatusCode.Created, user);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        public KeyValuePair<StatusCode, object> Login(User user)
        {
            try
            {
                if (_userService.IsRegistered(user))
                {
                    MtcgClient client;
                    if ((client = _userService.Login(user)) !=null)
                    {
                        ClientMapSingleton.GetInstance.ClientMap.AddOrUpdate(client.SessionToken, client,
                            (key, oldValue) => client);
                    }
                    return new KeyValuePair<StatusCode, object>(StatusCode.OK, client);
                }
                
                return new KeyValuePair<StatusCode, object>(StatusCode.NotFound, null);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        //TODO work with /transactions --> Inject UserServiceWithTransactions
        public bool AcquirePackage(User user)
        {
            if (user == null) return false;

            int packagePrice = _userService.GetPackagePrice();
            if (!HasEnoughCoins(user.Coins, packagePrice)) return false;
            Package package = _userService.AcquirePackage();
            user.AddPackage(package);
            user.RemoveCoins(packagePrice);
            return true;

        }

        #endregion

    }
}
