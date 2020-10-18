using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Controller
{
    public class BattleController
    {

        #region fields
        private IBattleService _battleService;
        private List<Battle> _currentBattles = new List<Battle>();
        #endregion

        #region properties
        #endregion

        #region constructor

        public BattleController(IBattleService battleService)
        {
            _battleService = battleService;
        }
        #endregion

        #region private methods

        #endregion

        #region public methods

        public bool StartBattle(Battle battle)
        {
            _currentBattles.Add(battle);
            //Do this in a new Task with a cancellation Token
            BattleResult battleResult = _battleService.StartBattle(battle.Player1, battle.Player2);
            bool finishedCorrectly = true;

            switch (battleResult)
            {
                case BattleResult.Player1Wins:
                {
                    _battleService.CalculateAndApplyMmr(battle.Player1, battle.Player2);
                    break;
                }
                case BattleResult.Player2Wins:
                {
                    _battleService.CalculateAndApplyMmr(battle.Player2, battle.Player1);
                    break;
                }
                case BattleResult.Cancelled:
                default:
                {
                    finishedCorrectly=false;
                    break;
                }
            }

            _currentBattles.Remove(battle);
            return finishedCorrectly;
        }

        public void CancelBattle()
        {
            //Request to cancel the Task
            throw new NotImplementedException();
        }
        #endregion

    }
}
