using NUnit.Framework;

namespace SWE1_MTCG.Test
{
    [TestFixture]
    public class UserTest
    {
        private User _user;

        [SetUp]
        public void SetUp()
        {
            //common arrange
            _user = new User("Robert Haslinger", "testPassword");
        }

        [Test]
        public void Test_UserLoginShouldReturnFalseWhenNotRegistered()
        {
            //act
            bool isLoggedIn = _user.Login();

            //assert
            Assert.IsFalse(isLoggedIn);
        }

        [Test]
        public void Test_UserShouldBeRegisteredAfterRegister()
        {
            //act
            _user.Register();
            bool isRegistered = _user.IsRegistered();

            //assert
            Assert.IsTrue(isRegistered);
        }

        [TestCase(0)]
        [TestCase(5)]
        public void Test_UserAcquirePackageShouldReturnFalseWhenUserCoinsLessThan20(int coins)
        {
            //act
            _user.AddCoins(coins);
            bool hasPackageAcquired = UserTest.AcquirePackage();

            //assert
            Assert.IsFalse(hasPackageAcquired);
        }

        [TestCase(20)]
        [TestCase(40)]
        public void Test_UserAcquirePackageShouldReturnTrueWhenUserCoins20OrGreater(int coins)
        {
            //act
            _user.AddCoins(coins);
            bool hasPackageAcquired = UserTest.AcquirePackage();

            //assert
            Assert.IsTrue(hasPackageAcquired);
        }


    }
}