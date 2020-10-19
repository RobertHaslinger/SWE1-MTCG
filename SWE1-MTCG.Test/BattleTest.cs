using Moq;
using NUnit.Framework;
using SWE1_MTCG.Battle;
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
        private User _player1;
        private User _player2;

        [SetUp]
        public void SetUp()
        {
            _battleServiceMock = new Mock<IBattleService>();
            _battleController= new BattleController(_battleServiceMock.Object);
            _player1= new User("Player 1", "myPassword");
            _player2 = new User("Player 2", "myPassword");
        }

        [Test]
        public void Test_BattleControllerShouldNotCalculateMmrIfBattleResultIsCancelled()
        {
            _battleServiceMock.Setup(s => s.StartBattle(_player1, _player2)).Returns(BattleResult.Cancelled);

            _battleController.StartBattle(new BattleBase(_player1, _player2));
            _battleController.CancelBattle(_player1);

            _battleServiceMock.Verify(s => s.CalculateAndApplyMmr(_player1, _player2), Times.Never);
        }

        [Test]
        public void Test_BattleControllerShouldNotCalculateMmrIfBattleResultIsDraw()
        {
            _battleServiceMock.Setup(s => s.StartBattle(_player1, _player2)).Returns(BattleResult.Draw);

            _battleController.StartBattle(new BattleBase(_player1, _player2));

            _battleServiceMock.Verify(s => s.CalculateAndApplyMmr(_player1, _player2), Times.Never);
        }

        [TestCase(BattleResult.Player1Wins)]
        [TestCase(BattleResult.Player2Wins)]
        public void Test_BattleControllerShouldCalculateMmrIfBattleResultIsPlayer1WinsOrPlayer2Wins(BattleResult battleResult)
        {
            _battleServiceMock.Setup(s => s.StartBattle(_player1, _player2)).Returns(battleResult);

            _battleController.StartBattle(new BattleBase(_player1, _player2));

            if (battleResult == BattleResult.Player1Wins)
            {
                _battleServiceMock.Verify(s => s.CalculateAndApplyMmr(_player1, _player2), Times.Once);
            }
            else
            {
                _battleServiceMock.Verify(s => s.CalculateAndApplyMmr(_player2, _player1), Times.Once);
            }
        }
    }
}