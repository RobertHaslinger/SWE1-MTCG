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
    public class UserController
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

        /// <summary>
        /// PostgreSQL Error Codes: https://www.postgresql.org/docs/current/errcodes-appendix.html
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private KeyValuePair<StatusCode, object> HandleException(Exception ex)
        {
            Console.WriteLine(ex.Message);
            
            if (ex is NpgsqlException)
            {
                return ((NpgsqlException) ex).SqlState switch
                {
                    { } state when state.StartsWith("02") => new KeyValuePair<StatusCode, object>(StatusCode.BadRequest, null),
                    { } state when state.StartsWith("08") => new KeyValuePair<StatusCode, object>(StatusCode.NotFound, null),
                    { } state when state.StartsWith("0A") => new KeyValuePair<StatusCode, object>(StatusCode.NotImplemented, null),
                    { } state when state.StartsWith("23") => new KeyValuePair<StatusCode, object>(StatusCode.Conflict, null),
                    _ => new KeyValuePair<StatusCode, object>(StatusCode.InternalServerError, null)
                };
            }

            return new KeyValuePair<StatusCode, object>(StatusCode.InternalServerError, null);
        }

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
