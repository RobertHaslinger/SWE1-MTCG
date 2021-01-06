using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Battle;
using SWE1_MTCG.Client;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Services
{
    public interface IBattleService
    {
        KeyValuePair<BattleResult, BattleLog> StartBattle(User player1, User player2);

    }
}
