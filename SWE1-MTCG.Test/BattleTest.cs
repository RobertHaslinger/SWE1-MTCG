using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SWE1_MTCG.Battle;
using SWE1_MTCG.Client;
using SWE1_MTCG.Controller;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Test
{
    [TestFixture]
    public class BattleTest
    {
        private Mock<IBattleService> _battleServiceMock;
        private BattleController _battleController;
        private MtcgClient _player1;
        private MtcgClient _player2;

        [SetUp]
        public void SetUp()
        {
            _battleServiceMock = new Mock<IBattleService>();
            _battleController= new BattleController(_battleServiceMock.Object);
            _player1= new MtcgClient(new User("Player 1", "myPassword"), "1");
            _player2 = new MtcgClient(new User("Player 2", "myPassword"), "2");
        }
        
        /* Obsolete because I moved the elo calculation to the BattleController
        [Test]
        public void Test_BattleControllerShouldNotCalculateMmrIfBattleResultIsDraw()
        {
            _battleServiceMock.Setup(s => s.StartBattle(_player1.User, _player2.User)).Returns(() => new KeyValuePair<BattleResult, BattleLog>(BattleResult.Draw, null));

            _battleController.StartBattle(new BattleBase(ref _player1, ref _player2));

            _battleServiceMock.Verify(s => s.CalculateAndApplyMmr(_player1.User, _player2.User), Times.Never);
        }

        [TestCase(BattleResult.Player1Wins)]
        [TestCase(BattleResult.Player2Wins)]
        public void Test_BattleControllerShouldCalculateMmrIfBattleResultIsPlayer1WinsOrPlayer2Wins(BattleResult battleResult)
        {
            _battleServiceMock.Setup(s => s.StartBattle(_player1.User, _player2.User)).Returns(() => new KeyValuePair<BattleResult, BattleLog>(battleResult, null));

            _battleController.StartBattle(new BattleBase(ref _player1, ref _player2));

            if (battleResult == BattleResult.Player1Wins)
            {
                _battleServiceMock.Verify(s => s.CalculateAndApplyMmr(_player1.User, _player2.User), Times.Once);
            }
            else
            {
                _battleServiceMock.Verify(s => s.CalculateAndApplyMmr(_player2.User, _player1.User), Times.Once);
            }
        }*/
    }
}