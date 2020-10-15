using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Controller
{
    public class UserController
    {

        #region fields

        private IUserService _userService;

        #endregion

        #region properties

        public  User User { get; private set; }
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

        public void Register(string username, string password)
        {
            User user = new User(username, password);
            _userService.Register(user);
        }


        #endregion

        public bool Login(User user)
        {
            if (_userService.IsRegistered(user))
            {
                User = _userService.Login(user);
            }

            return User != null;
        }

        public bool AcquirePackage()
        {
            if (User == null) return false;

            int packagePrice = _userService.GetPackagePrice();
            if (HasEnoughCoins(User.Coins, packagePrice))
            {
                Package package = _userService.AcquirePackage();
                User.AddPackage(package);
                User.RemoveCoins(packagePrice);
                return true;
            }

            return false;
        }
    }
}
