using System;
using System.Collections.Generic;
using System.Text;

namespace SWE1_MTCG
{
    public class User
    {
        #region fields

        private string _credentials;

        #endregion


        #region constructor

        public User(string username, string password)
        {
            _credentials = $"{username}:{Hash(password)}";
        }

        #endregion


        #region private methods

        /// <summary>
        /// Takes input parameter and returns a SHA512 Hash.
        /// Source: https://stackoverflow.com/a/33546720/10888504
        /// </summary>
        /// <param name="stringToHash"></param>
        /// <returns></returns>
        private string Hash(string stringToHash)
        {
            byte[] bytes = new UTF8Encoding().GetBytes(stringToHash);
            using var algorithm = new System.Security.Cryptography.SHA512Managed();
            var hashBytes = algorithm.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }

        #endregion

        #region public methods

        public bool Login()
        {
            throw new NotImplementedException();
        }

        #endregion


        public void Register()
        {
            throw new NotImplementedException();
        }

        public bool IsRegistered()
        {
            throw new NotImplementedException();
        }

        public void AddCoins(in int coins)
        {
            throw new NotImplementedException();
        }

        public bool AcquirePackage()
        {
            throw new NotImplementedException();
        }
    }
}
