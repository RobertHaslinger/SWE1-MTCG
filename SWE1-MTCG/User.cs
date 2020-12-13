using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Npgsql;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.JsonConverter;
using SWE1_MTCG.Services;

namespace SWE1_MTCG
{
    public class User
    {
        #region fields
        
        
        #endregion

        #region properties

        public string Username { get; init; }
        public string Credentials { get; init; }
        public Profile Profile { get; set; }
        [JsonIgnore]
        public int UserId { get; }
        [JsonIgnore]
        public int Coins { get; private set; } = 0;
        [JsonIgnore]
        public Deck Deck { get; set; } = new Deck();
        [JsonIgnore]
        public CardStack Stack { get; set; } = new CardStack();
        [JsonIgnore]
        public Stack<Package> CurrentUnopenedPackages { get; set; } = new Stack<Package>();
        #endregion


        #region constructor

        public User(string username, string password)
        {
            Username = username;
            Credentials = $"{username}:{Hash(password)}";
        }

        public User(NpgsqlDataReader reader)
        {
            UserId = (int) reader["Id"];
            Username = reader["Username"].ToString();
            Credentials = $"{Username}:{Encoding.Default.GetString((byte[])reader["Password_Hash"])}";
            Coins = (int)reader["Coins"];
            string unopenedPackages;
            if (!string.IsNullOrWhiteSpace(unopenedPackages = reader["UnopenedPackages"].ToString()))
            {
                CurrentUnopenedPackages = JsonSerializer.Deserialize<Stack<Package>>(unopenedPackages );
            }

            string stack;
            if (!string.IsNullOrWhiteSpace(stack = reader["Stack"].ToString()))
            {
                Stack.AddCards(JsonSerializer.Deserialize<List<Card>>(stack));
            }

            string deck;
            if (!string.IsNullOrWhiteSpace(deck = reader["Deck"].ToString()))
            {
                Deck.AddCards(JsonSerializer.Deserialize<List<Card>>(deck));
            }

            string profile;
            if (!string.IsNullOrWhiteSpace(profile = reader["Profile"].ToString()))
            {
                Profile = JsonSerializer.Deserialize<Profile>(profile);
            }
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
            if (amount > 0)
            {
                Coins -= amount;
            }
        }

        public void AddPackage(Package package)
        {
            if (package == null) return;

            CurrentUnopenedPackages.Push(package);
        }

        public IEnumerable OpenPackage()
        {
            if (!HasAnyUnopenedPackages()) return null;

            return CurrentUnopenedPackages.Pop().GetAllCards();
        }

        public bool HasAnyUnopenedPackages()
        {
            return CurrentUnopenedPackages.Count > 0;
        }

        #endregion



    }
}
