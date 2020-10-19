using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using SWE1_MTCG.Battle;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Controller
{
    public class BattleController
    {

        #region fields
        private IBattleService _battleService;
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

        public bool StartBattle(BattleBase battle)
        {
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
                default:
                {
                    finishedCorrectly=false;
                    break;
                }
            }
            return finishedCorrectly;
        }
        #endregion

    }
}
