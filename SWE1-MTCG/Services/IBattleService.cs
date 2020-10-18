using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Services
{
    public interface IBattleService
    {
        void CalculateAndApplyMmr(User winner, User loser);
        BattleResult StartBattle(User player1, User player2);

    }
}
