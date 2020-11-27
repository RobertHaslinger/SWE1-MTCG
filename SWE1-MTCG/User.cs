using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Services;

namespace SWE1_MTCG
{
    public class User
    {
        #region fields
        
        private Stack<Package> _currentUnopenedPackages = new Stack<Package>();
        #endregion

        #region properties

        public string Username { get; init; }
        public string Credentials { get; init; }
        public int Coins { get; private set; } = 0;
        public Deck Deck { get; set; } = new Deck();
        public CardStack Stack { get; set; } = new CardStack();

        #endregion


        #region constructor

        public User(string username, string password)
        {
            Username = username;
            Credentials = $"{username}:{Hash(password)}";
        }

        //TODO add Deck, Stack, ...
        public User(NpgsqlDataReader reader)
        {
            Username = reader.GetValue("Username").ToString();
            Credentials = $"{Username}:{reader.GetValue("Password_hash")}";
            Coins = (int)reader.GetValue("Coins");
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

        public void AddCoins(int amount)
        {
            if (amount > 0)
            {
                Coins += amount;
            }
        }

        public void RemoveCoins(int amount)
        {
            if (amount < 0)
            {
                Coins -= amount;
            }
        }

        public void AddPackage(Package package)
        {
            if (package == null) return;

            _currentUnopenedPackages.Push(package);
        }

        public IEnumerable OpenPackage()
        {
            if (!HasAnyUnopenedPackages()) return null;

            return _currentUnopenedPackages.Pop().GetAllCards();
        }

        public bool HasAnyUnopenedPackages()
        {
            return _currentUnopenedPackages.Count > 0;
        }

        #endregion



    }
}
