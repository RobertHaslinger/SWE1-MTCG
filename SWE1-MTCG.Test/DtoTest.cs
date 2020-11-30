using System;
using NUnit.Framework;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Cards.Monster;
using SWE1_MTCG.Cards.Spells;
using SWE1_MTCG.Dto;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Test.TestCaseData;

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

        [Test, TestCaseSource(typeof(CardDtoTestCaseData), nameof(CardDtoTestCaseData.TestCases))]
        public void Test_CardDtoToObjectReturnsCorrectProperties(CardDto testDto, Card result)
        {
            Card card = testDto.ToObject();
            
            Assert.AreEqual(result?.Name, card?.Name);
            Assert.AreEqual(result?.Damage, card?.Damage);
            Assert.AreEqual(result?.Element, card?.Element);
        }
    }
}