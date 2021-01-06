using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Npgsql;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Client;
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

        public KeyValuePair<StatusCode, object> ViewProfile(string username)
        {
            try
            {
                Profile profile;
                if ((profile= _userService.ViewProfile(username))==null)
                    return new KeyValuePair<StatusCode, object>(StatusCode.NotFound, "");
                return new KeyValuePair<StatusCode, object>(StatusCode.OK, profile);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        public KeyValuePair<StatusCode, object> EditProfile(ref MtcgClient client, Profile profile)
        {
            try
            {
                if (_userService.EditProfile(ref client, profile))
                {
                    return new KeyValuePair<StatusCode, object>(StatusCode.OK, profile);
                }
                return new KeyValuePair<StatusCode, object>(StatusCode.InternalServerError, "Something went wrong");
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        public KeyValuePair<StatusCode, object> ViewStats(ref MtcgClient client)
        {
            try
            {
                return new KeyValuePair<StatusCode, object>(StatusCode.OK, client.User.Stats);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        public KeyValuePair<StatusCode, object> ViewScoreboard(MtcgClient client)
        {
            try
            {
                return new KeyValuePair<StatusCode, object>(StatusCode.OK, _userService.GetScoreboard(client));
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        public KeyValuePair<StatusCode, object> AcquirePackage(ref MtcgClient client, PackageType type)
        {
            if (!(_userService is IPackageTransactionService && _userService is ILoggable))
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
                string logDescription =
                    $"{client.User.Username} acquired a {Enum.GetName(typeof(PackageType), type)} package and paid {packagePrice}.";
                bool logged = ((ILoggable)_userService).Log(new Dictionary<string, object>(new []
                {
                    new KeyValuePair<string, object>("client", client),
                    new KeyValuePair<string, object>("description", logDescription) 
                }));


                if (!logged)
                    Console.WriteLine("Package transaction was not logged");

                return new KeyValuePair<StatusCode, object>(StatusCode.OK, logDescription);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        public KeyValuePair<StatusCode, object> OpenPackage(ref MtcgClient client)
        {
            try
            {
                if (!(_userService is IPackageOpenService))
                    return new KeyValuePair<StatusCode, object>(StatusCode.InternalServerError, null);

                if (client == null)
                    return new KeyValuePair<StatusCode, object>(StatusCode.Unauthorized, null);

                if (!client.User.HasAnyUnopenedPackages())
                {
                    return new KeyValuePair<StatusCode, object>(StatusCode.Conflict, "You have no unopened packages left. Go get some!");
                }

                Package package = client.User.CurrentUnopenedPackages.Pop();
                if (((IPackageOpenService)_userService).OpenPackage(ref client, package))
                {
                    return new KeyValuePair<StatusCode, object>(StatusCode.OK, package.GetAllCards());
                }

                return new KeyValuePair<StatusCode, object>(StatusCode.InternalServerError, "");
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        public KeyValuePair<StatusCode, object> ConfigureDeck(ref MtcgClient client, Deck deck)
        {
            try
            {
                if (client == null)
                    return new KeyValuePair<StatusCode, object>(StatusCode.Unauthorized, null);

                //save old deck to fallback if necessary
                Deck oldDeck = new Deck();
                foreach (Card card in client.User.Deck.GetAllCards())
                {
                    oldDeck.AddCard(card);
                }

                //clear user deck
                client.User.Deck.ClearDeck();

                foreach (Card card in deck.GetAllCards())
                {
                    if (!client.User.Stack.GetAllCards().Any(c => c.Guid.Equals(card.Guid)))
                    {
                        return new KeyValuePair<StatusCode, object>(StatusCode.BadRequest, "You do not possess this card. Acquire some packages and you might will someday.");
                    }

                    if (!client.User.Deck.AddCard(card))
                    {
                        client.User.Deck.ClearDeck();
                        client.User.Deck.AddCards(oldDeck.GetAllCards());
                        return new KeyValuePair<StatusCode, object>(StatusCode.BadRequest, "The given deck either contains more than 4 cards or has more than 2 same cards.");
                    }
                }

                if (((IDeckService)_userService).ConfigureDeck(ref client))
                {
                    return new KeyValuePair<StatusCode, object>(StatusCode.OK, client.User.Deck.GetAllCards());
                }

                client.User.Deck.ClearDeck();
                client.User.Deck.AddCards(oldDeck.GetAllCards());
                return new KeyValuePair<StatusCode, object>(StatusCode.InternalServerError, "");
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        #endregion

    }
}
