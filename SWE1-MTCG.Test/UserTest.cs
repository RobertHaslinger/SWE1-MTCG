using Moq;
using NUnit.Framework;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Controller;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Test
{
    [TestFixture]
    public class UserTest
    {
        private Mock<IUserService> _userServiceMock;
        private UserController _userController;
        private User _user;

        [SetUp]
        public void SetUp()
        {
            //common arrange
            _userServiceMock= new Mock<IUserService>(MockBehavior.Strict);
            _userController= new UserController(_userServiceMock.Object);
            _user= new User("Test User", "secret password");
        }

        [Test]
        public void Test_UserControllerLoginShouldReturnFalseWhenNotRegistered()
        {
            _userServiceMock.Setup(s => s.IsRegistered(_user)).Returns(false);

            bool isLoggedIn = _userController.Login(_user);

            Assert.IsFalse(isLoggedIn);
        }

        [Test]
        public void Test_UserControllerUserShouldNotBeNullAfterLoginWhenRegistered()
        {
            _userServiceMock.Setup(s => s.IsRegistered(_user)).Returns(true);
            _userServiceMock.Setup(s => s.Login(_user)).Returns(_user);

            _userController.Login(_user);

            Assert.IsNotNull(_userController.User);
        }

        [TestCase(0)]
        [TestCase(19)]
        public void Test_UserControllerAcquirePackageShouldBeCalledNeverWhenUserCoinsLessThan20(int coins)
        {
            _userServiceMock.Setup(s => s.IsRegistered(_user)).Returns(true);
            _userServiceMock.Setup(s => s.Login(_user)).Returns(_user);
            _userServiceMock.Setup(s => s.GetPackagePrice()).Returns(20);
            _userServiceMock.Setup(s => s.AcquirePackage()).Returns(new Package());

            _userController.Login(_user);

            _userController.User.AddCoins(coins);
            _userController.AcquirePackage();

            _userServiceMock.Verify(s => s.AcquirePackage(), Times.Never);
        }

        [TestCase(20)]
        [TestCase(40)]
        public void Test_UserControllerAcquirePackageShouldBeCalledOnceAndUserShouldHaveAtleastOnePackageWhenUserCoinsAtLeast20(int coins)
        {
            _userServiceMock.Setup(s => s.IsRegistered(_user)).Returns(true);
            _userServiceMock.Setup(s => s.Login(_user)).Returns(_user);
            _userServiceMock.Setup(s => s.GetPackagePrice()).Returns(20);
            _userServiceMock.Setup(s => s.AcquirePackage()).Returns(new Package());


            _userController.Login(_user);

            _userController.User.AddCoins(coins);
            _userController.AcquirePackage();

            _userServiceMock.Verify(s => s.AcquirePackage(), Times.Once);
            Assert.IsTrue(_userController.User.HasAnyUnopenedPackages());
        }


    }
}