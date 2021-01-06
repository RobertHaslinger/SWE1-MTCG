using System.Collections;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Cards.Monster;
using SWE1_MTCG.Cards.Spells;
using SWE1_MTCG.Client;
using SWE1_MTCG.Controller;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Services;
using SWE1_MTCG.Trade;

namespace SWE1_MTCG.Test
{
    [TestFixture]
    public class MarketTest
    {
        private Mock<IMarketService> _marketService;
        private MarketController _marketController;

        [SetUp]
        public void SetUp()
        {
            _marketService = new Mock<IMarketService>();
            _marketController= new MarketController(_marketService.Object);
        }

        [Test]
        public void Test_MarketControllerShouldReturnEmptyIEnumerableWhenUnknownUser()
        {
            User fakeUser = new User("unknown", "wrongPassword");
            _marketService.Setup(s => s.GetOpenTradingDealsForUser(fakeUser))
                .Returns(new List<Card>());

            IEnumerable allTradingDealsForUser = _marketController.GetOpenTradingDealsForUser(fakeUser);
            Assert.IsEmpty(allTradingDealsForUser);
        }

        [Test]
        public void Test_MarketControllerShouldNotAddTradingDealIfNotValid()
        {
            TradingDeal deal= new TradingDeal(new User("myUser", "myPassword"));
            _marketService.Setup(s => s.AddTradingDeal(deal)).Returns(true);

            _marketController.AddTradingDeal(deal);

            _marketService.Verify(s => s.AddTradingDeal(deal), Times.Never);
        }

        [Test]
        public void Test_MarketControllerShouldNotProcessTradeIfItDoesntMatchRequirements()
        {
            TradingDeal deal = new TradingDeal(new User("myUser", "myPassword"));
            Card fireDragon= new Dragon("Fire Dragon", 40, ElementType.Fire);
            deal.AddOffer(fireDragon.GetCardStat());
            deal.AddRequest(new CardStat("", 35, ElementType.Water));
            Card cascade= new WaterSpell("Spit", 10);
            CardStat bid = cascade.GetCardStat();
            _marketService.Setup(s => s.Trade(deal,bid)).Returns(false);

            _marketController.ProcessTrade(deal, bid);

            _marketService.Verify(s => s.Trade(deal, bid), Times.Never);
        }

        [Test]
        public void Test_MarketControllerShouldProcessTradeIfItMatchesRequirements()
        {
            TradingDeal deal = new TradingDeal(new User("myUser", "myPassword"));
            Card fireDragon = new Dragon("Fire Dragon", 40, ElementType.Fire);
            deal.AddOffer(fireDragon.GetCardStat());
            deal.AddRequest(new CardStat("", 35, ElementType.Water));
            Card cascade = new WaterSpell("Mega Spit", 60);
            CardStat bid = cascade.GetCardStat();
            _marketService.Setup(s => s.Trade(deal, bid)).Returns(false);

            _marketController.ProcessTrade(deal, bid);

            _marketService.Verify(s => s.Trade(deal, bid), Times.Once);
        }
    }
}