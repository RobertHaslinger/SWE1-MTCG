using Moq;
using NUnit.Framework;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Controller;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Test
{
    [TestFixture]
    public class UserControllerTest
    {
        private Mock<IUserService> _userServiceMock;
        private UserController _userController;
        private MtcgClient _client;
        private User _user;

        [SetUp]
        public void SetUp()
        {
            //common arrange
            _userServiceMock= new Mock<IUserService>(MockBehavior.Strict);
            _userController= new UserController(_userServiceMock.Object);
            _user = new User("Test User", "secret password");
            _client = new MtcgClient(_user, "Test user-mtcgToken");
        }

        [Test]
        public void Test_UserControllerLoginShouldReturnFalseWhenNotRegistered()
        {
            _userServiceMock.Setup(s => s.IsRegistered(_user)).Returns(false);

            StatusCode response = _userController.Login(_user).Key;

            Assert.AreEqual(StatusCode.NotFound, response);
        }

        [Test]
        public void Test_UserControllerClientShouldNotBeNullAfterLoginWhenRegistered()
        {
            _userServiceMock.Setup(s => s.IsRegistered(_user)).Returns(true);
            _userServiceMock.Setup(s => s.Login(_user)).Returns(_client);

            StatusCode response = _userController.Login(_user).Key;

            Assert.AreEqual(StatusCode.OK, response);
        }
    }
}