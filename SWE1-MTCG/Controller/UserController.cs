using System;
using System.Collections.Generic;
using System.Net.Sockets;
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

        public KeyValuePair<StatusCode, object> Login(User user, TcpClient socket)
        {
            try
            {
                if (_userService.IsRegistered(user))
                {
                    MtcgClient client;
                    if ((client = _userService.Login(user)) !=null)
                    {
                        client.Socket = socket;
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

        public KeyValuePair<StatusCode, object> AcquirePackage(ref MtcgClient client, PackageType type)
        {
            if (!(_userService is IPackageTransactionService))
                return new KeyValuePair<StatusCode, object>(StatusCode.InternalServerError, null);

            if (client == null) 
                return new KeyValuePair<StatusCode, object>(StatusCode.Unauthorized, null);

            try
            {
                int packagePrice = ((IPackageTransactionService)_userService).GetPackagePrice(type);
                if (packagePrice == -1)
                {
                    return new KeyValuePair<StatusCode, object>(StatusCode.InternalServerError, null);
                }
                if (!HasEnoughCoins(client.User.Coins, packagePrice))
                    return new KeyValuePair<StatusCode, object>(StatusCode.Conflict, "You don't have enough coins");

                ((IPackageTransactionService)_userService).AcquirePackage(ref client, type);
                return new KeyValuePair<StatusCode, object>(StatusCode.OK, "");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion

    }
}
