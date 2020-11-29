using System;
using NUnit.Framework;
using SWE1_MTCG.Cards.Monster;
using SWE1_MTCG.Cards.Spells;
using SWE1_MTCG.Dto;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Test
{
    [TestFixture]
    public class DtoTest
    {
        [Test]
        public void Test_UserDtoToObjectReturnsUserWithSameValues()
        {
            UserDto userDto= new UserDto()
            {
                Password = "testPassword",
                Username = "testUsername"
            };
            User expectedUser= new User("testUsername", "testPassword");

            User realUser = userDto.ToObject();

            Assert.IsTrue(expectedUser.Credentials==realUser.Credentials);
        }

        [Test]
        [TestCase("Dragon", typeof(Dragon))]
        [TestCase("FireSpell", typeof(FireSpell))]
        [TestCase("Knight", typeof(Knight))]
        public void Test_CardDtoToObjectReturnsCorrectCardType(string typename, Type expectedType)
        {
            CardDto cardDto= new CardDto()
            {
                CardType = typename,
                Damage = 20,
                Name = "Testcard"

            };

            Type t = cardDto.ToObject().GetType();

            Assert.AreEqual(expectedType, t);
        }
    }
}