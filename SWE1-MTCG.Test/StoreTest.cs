using System;
using System.Collections;
using NUnit.Framework;
using SWE1_MTCG.Trade;

namespace SWE1_MTCG.Test
{
    [TestFixture]
    public class StoreTest
    {

        private Store _store;

        [SetUp]
        public void SetUp()
        {
            //common arrange
            //TODO add WebApi in constructor after WebService project is finished
            _store= new Store();
        }

        [TestCase("unknownUser")]
        public void Test_StoreGetOpenTradingDealsShouldReturnEmptyIEnumerableWhenUnknownUser(string username)
        {
            //act
            IEnumerable tradingDealsForUser = _store.GetOpenTradingDealsByUser(username);

            //assert
            Assert.IsEmpty(tradingDealsForUser);
        }

        [Test]
        public void Test_StoreShouldReturnAtleastOneTradingDealForUserAfterAdding()
        {
            //arrange
            User requestor = new User("testUser", "testPassword");
            TradingDeal dealToAdd = new TradingDeal(requestor);

            //act
            _store.AddTradingDeal(dealToAdd);
            IEnumerable tradingDeals = _store.GetOpenTradingDealsByUser(dealToAdd.GetRequestor());

            //assert
            Assert.IsNotEmpty(tradingDeals);
        }

        [Test]
        public void Test_StoreShouldReturnAtleastOneTradingDealAfterAdding()
        {
            //arrange
            User requestor= new User("testUser", "testPassword");
            TradingDeal dealToAdd= new TradingDeal(requestor);
            
            //act
            _store.AddTradingDeal(dealToAdd);
            IEnumerable tradingDeals = _store.GetAllOpenTradingDeals();

            //assert
            Assert.IsNotEmpty(tradingDeals);
        }

        [Test]
        public void Test_StoreShouldOnlyListTradingDealsThatMatchRequirements()
        {
            throw new NotImplementedException();
        }
    }
}