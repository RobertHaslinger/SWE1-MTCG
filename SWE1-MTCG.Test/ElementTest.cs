using System;
using NUnit.Framework;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Interfaces;
using SWE1_MTCG.Services;

namespace SWE1_MTCG.Test
{
    [TestFixture]
    public class ElementTest
    {

        private IElementService _elementService;

        [SetUp]
        public void SetUp()
        {
            //common arrange
            _elementService = new ElementServiceBase();
        }

        [Test]
        public void Test_ElementFireShouldBeEffectiveAgainstNormal()
        {
            double effectiveness = _elementService.CompareElement(ElementType.Fire, ElementType.Normal);
            Assert.AreEqual(effectiveness, ElementEffectiveness.Effective);
        }

        [Test]
        public void Test_ElementFireShouldNotBeEffectiveAgainstWater()
        {
            double effectiveness = _elementService.CompareElement(ElementType.Fire, ElementType.Water);
            Assert.AreEqual(effectiveness, ElementEffectiveness.NotEffective);
        }

        [Test]
        public void Test_ElementFireShouldBeNormalAgainstFire()
        {
            double effectiveness = _elementService.CompareElement(ElementType.Fire, ElementType.Fire);
            Assert.AreEqual(effectiveness, ElementEffectiveness.Normal);
        }

        [Test]
        public void Test_ElementNormalShouldBeEffectiveAgainstWater()
        {
            double effectiveness = _elementService.CompareElement(ElementType.Normal, ElementType.Water);
            Assert.AreEqual(effectiveness, ElementEffectiveness.Effective);
        }

        [Test]
        public void Test_ElementNormalShouldNotBeEffetiveAgainstFire()
        {
            double effectiveness = _elementService.CompareElement(ElementType.Normal, ElementType.Fire);
            Assert.AreEqual(effectiveness, ElementEffectiveness.NotEffective);
        }

        [Test]
        public void Test_ElementNormalShouldBeNormalAgainstNormal()
        {
            double effectiveness = _elementService.CompareElement(ElementType.Normal, ElementType.Normal);
            Assert.AreEqual(effectiveness, ElementEffectiveness.Normal);
        }

        [Test]
        public void Test_ElementWaterShouldBeEffetiveAgainstFire()
        {
            double effectiveness = _elementService.CompareElement(ElementType.Water, ElementType.Fire);
            Assert.AreEqual(effectiveness, ElementEffectiveness.Effective);
        }

        [Test]
        public void Test_ElementWaterShouldNotBeEffectiveAgainstNormal()
        {
            double effectiveness = _elementService.CompareElement(ElementType.Water, ElementType.Normal);
            Assert.AreEqual(effectiveness, ElementEffectiveness.NotEffective);
        }

        [Test]
        public void Test_ElementWaterShouldBeNormalAgainstWater()
        {
            double effectiveness = _elementService.CompareElement(ElementType.Water, ElementType.Water);
            Assert.AreEqual(effectiveness, ElementEffectiveness.Normal);
        }
    }
}