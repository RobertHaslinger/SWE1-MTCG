using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using SWE1_MTCG.Battle;
using SWE1_MTCG.Client;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Server;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Controller
{
    public class BattleController : ControllerWithDbAccess
    {

        #region fields
        private IBattleService _battleService;
        private UserController _userController;

        #endregion

        #region properties
        #endregion

        #region constructor

        public BattleController(IBattleService battleService)
        {
            _battleService = battleService;
            _userController= new UserController(new UserService());
        }
        #endregion

        #region private methods

        #endregion

        #region public methods

        public KeyValuePair<StatusCode, object> StartBattle(BattleBase battle)
        {
            try
            {
                KeyValuePair<BattleResult, BattleLog> result = _battleService.StartBattle(battle.Player1.User, battle.Player2.User);

                switch (result.Key)
                {
                    case BattleResult.Player1Wins:
                    {
                        int delta = CalculateEloDelta(battle.Player1.User, battle.Player2.User);
                        battle.Player1.User.Stats.Elo += delta;
                        battle.Player2.User.Stats.Elo -= delta;
                        battle.Player1.User.Stats.Logs.Add(result.Value);
                        battle.Player1.User.Stats.Wins++;
                        battle.Player2.User.Stats.Logs.Add(result.Value);
                        battle.Player2.User.Stats.Losses++;
                        break;
                    }
                    case BattleResult.Player2Wins:
                    {
                        int delta = CalculateEloDelta(battle.Player2.User, battle.Player1.User);
                        battle.Player2.User.Stats.Elo += delta;
                        battle.Player1.User.Stats.Elo -= delta;
                        battle.Player2.User.Stats.Logs.Add(result.Value);
                        battle.Player2.User.Stats.Wins++;
                        battle.Player1.User.Stats.Logs.Add(result.Value);
                        battle.Player1.User.Stats.Losses++;
                        break;
                    }
                    default:
                    {
                        battle.Player1.User.Stats.Logs.Add(result.Value);
                        battle.Player1.User.Stats.Draws++;
                        battle.Player2.User.Stats.Logs.Add(result.Value);
                        battle.Player2.User.Stats.Draws++;
                        break;
                    }
                }

                _userController.EditStats(battle.Player1);
                _userController.EditStats(battle.Player2);
                battle.Player2.CurrentBattleLog = new KeyValuePair<StatusCode, object>(StatusCode.OK, result.Value);
                ClientMapSingleton.GetInstance.ClientMap.AddOrUpdate(battle.Player2.SessionToken, battle.Player2,
                    (key, oldValue) => battle.Player2);
                return new KeyValuePair<StatusCode, object>(StatusCode.OK, result.Value);
            }
            catch (Exception e)
            {
                battle.Player2.CurrentBattleLog = HandleException(e);
                ClientMapSingleton.GetInstance.ClientMap.AddOrUpdate(battle.Player2.SessionToken, battle.Player2,
                    (key, oldValue) => battle.Player2);
                return HandleException(e);
            }
            
        }

        /// <summary>
        /// Elo calculation source: https://dotnetcoretutorials.com/2018/09/18/calculating-elo-in-c/
        /// </summary>
        /// <param name="winner"></param>
        /// <param name="loser"></param>
        private int CalculateEloDelta(User winner, User loser)
        {
            //the "k" should be between 10 (fluctuate) and 32 (slow)
            int eloK = 27;

            return (int)(eloK * (1 - ExpectationToWin(winner.Stats.Elo, loser.Stats.Elo)));
        }

        private double ExpectationToWin(int playerOneRating, int playerTwoRating)
        {
            return 1 / (1 + Math.Pow(10, (playerTwoRating - playerOneRating) / 400.0));
        }

        #endregion

    }
}
