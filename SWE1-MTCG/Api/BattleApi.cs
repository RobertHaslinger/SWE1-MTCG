using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SWE1_MTCG.Battle;
using SWE1_MTCG.Client;
using SWE1_MTCG.Controller;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Server;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Api
{
    public class BattleApi : IRestApi
    {
        private BattleController _battleController;
        private UserController _userController;
        public bool AllowAnonymous => false;

        public BattleApi()
        {
            IBattleService battleService= new BattleService(new ElementServiceBase());
            _battleController= new BattleController(battleService);
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
            MtcgClient client = (MtcgClient)param["client"];
            MtcgClient waitingClient=null;
            foreach (KeyValuePair<string, MtcgClient> clientPair in ClientMapSingleton.GetInstance.ClientMap)
            {
                if (clientPair.Value.IsReadyForBattle)
                {
                    waitingClient = clientPair.Value;
                    break;
                }
            }

            if (waitingClient == null)
            {
                if (client.User.Deck.GetAllCards().Count()!=4)
                    return new ResponseContext(request, new KeyValuePair<StatusCode, object>(StatusCode.BadRequest, "You have an invalid deck. Configure your deck and try again"));
                client.IsReadyForBattle = true;
                ClientMapSingleton.GetInstance.ClientMap.AddOrUpdate(client.SessionToken, client,
                    (key, oldValue) => client);

                while (client.IsReadyForBattle == true && client.CurrentBattleLog.Value==null)
                {
                    Thread.Sleep(100);
                    client = ClientMapSingleton.GetInstance.ClientMap[client.SessionToken];
                }

                KeyValuePair<StatusCode, object> responsePair= new KeyValuePair<StatusCode, object>(client.CurrentBattleLog.Key, client.CurrentBattleLog.Value);
                //reset battle log and ready for battle
                ResetBattleProperties(client);
                
                return new ResponseContext(request, responsePair);
            }

            BattleBase battle= new BattleBase(ref client, ref waitingClient);
            if (!battle.CheckDecks())
            {
                ResetBattleProperties(client);
                return new ResponseContext(request, new KeyValuePair<StatusCode, object>(StatusCode.BadRequest, "You have an invalid deck. Configure your deck and try again"));
            }
            ResetBattleProperties(client);
            return new ResponseContext(request, _battleController.StartBattle(battle));
        }

        private void ResetBattleProperties(MtcgClient client)
        {
            client.IsReadyForBattle = false;
            client.CurrentBattleLog = new KeyValuePair<StatusCode, object>(StatusCode.OK, null);
            ClientMapSingleton.GetInstance.ClientMap.AddOrUpdate(client.SessionToken, client,
                (key, oldValue) => client);
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
