using System.Collections;
using NUnit.Framework;

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
        public void Test_StoreGetOpenTradingDealsShouldReturnEmptyIEnumberableWhenUnknownUser(string username)
        {
            //act
            IEnumerable tradingDealsForUser = _store.GetOpenTradingDealsByUser(username);

            //assert
            Assert.IsEmpty(tradingDealsForUser);
        }
    }
}