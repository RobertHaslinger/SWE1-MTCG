using NUnit.Framework;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Cards.Monster;
using SWE1_MTCG.Cards.Spells;
using SWE1_MTCG.Enums;

namespace SWE1_MTCG.Test
{
    [TestFixture]
    public class CardTest
    {

        [Test]
        public void Test_WizardShouldControlOrk()
        {
            //arrange
            Card wizard= new Wizard("Gandalf", 50, ElementType.Fire);
            Card possibleOrc= new Orc("Grosssh", 20, ElementType.Normal);

            //act
            bool enemyControlled = ((Wizard)wizard).TryControlOrc(possibleOrc);

            //assert
            Assert.IsTrue(enemyControlled);
        }

        [Test]
        public void Test_WizardShouldNotControlNonOrc()
        {
            //arrange
            Card wizard = new Wizard("Gandalf", 50, ElementType.Fire);
            Card nonOrc = new Dragon("Butterfly", 20, ElementType.Normal);

            //act
            bool enemyControlled = ((Wizard)wizard).TryControlOrc(nonOrc);

            //assert
            Assert.IsFalse(enemyControlled);
        }

        [Test]
        public void Test_GoblinShouldBeScaredOfDragon()
        {
            //arrange
            Card goblin = new Goblin("Kimki", 50, ElementType.Fire);
            Card possibleDragon = new Dragon("Butterfly", 20, ElementType.Normal);

            //act
            bool isScared = ((Goblin)goblin).TryActScared(possibleDragon);

            //assert
            Assert.IsTrue(isScared);
        }

        [Test]
        public void Test_GoblinShouldNotBeScaredOfNonDragon()
        {
            //arrange
            Card goblin = new Goblin("Kimki", 50, ElementType.Fire);
            Card nonDragon = new Orc("Grosssh", 20, ElementType.Normal);

            //act
            bool isScared = ((Goblin)goblin).TryActScared(nonDragon);

            //assert
            Assert.IsFalse(isScared);
        }

        [Test]
        public void Test_KnightShouldDrownByWaterSpell()
        {
            //arrange
            Card knight = new Knight("Sir Rudolf", 50, ElementType.Normal);
            Card waterSpell = new WaterSpell("Cascade", 20);

            //act
            bool hasDrowned = ((Knight)knight).TryDrown(waterSpell);

            //assert
            Assert.IsTrue(hasDrowned);
        }

        [Test]
        public void Test_KnightShouldNotDrownByNonWaterSpell()
        {
            //arrange
            Card knight = new Knight("Sir Rudolf", 50, ElementType.Normal);
            Card nonWaterSpell = new NormalSpell("Mud Throw", 20);

            //act
            bool hasDrowned = ((Knight)knight).TryDrown(nonWaterSpell);

            //assert
            Assert.IsFalse(hasDrowned);
        }

        [Test]
        public void Test_KrakenShouldResistSpell()
        {
            //arrange
            Card kraken = new Kraken("Tentakel", 50, ElementType.Normal);
            Card normalSpell = new NormalSpell("Mud Throw", 20);

            //act
            bool hasResisted = ((Kraken)kraken).TryResistSpell(normalSpell);

            //assert
            Assert.IsTrue(hasResisted);
        }

        [Test]
        public void Test_KrakenShouldNotResistNonSpell()
        {
            //arrange
            Card kraken = new Kraken("Tentakel", 50, ElementType.Normal);
            Card nonSpell = new Dragon("Fire Dragon", 20, ElementType.Fire);

            //act
            bool hasResisted = ((Kraken)kraken).TryResistSpell(nonSpell);

            //assert
            Assert.IsFalse(hasResisted);
        }

        [Test]
        public void Test_FireElveShouldEvadeDragonAttack()
        {
            //arrange
            Card fireElve = new FireElf("Tentakel", 50, ElementType.Normal);
            Card dragon = new Dragon("Fire Dragon", 20, ElementType.Fire);

            //act
            bool hasEvaded = ((FireElf)fireElve).TryEvadeAttack(dragon);

            //assert
            Assert.IsTrue(hasEvaded);
        }

        [Test]
        public void Test_FireElveShouldNotEvadeNonDragonAttack()
        {
            //arrange
            Card fireElve = new FireElf("Tentakel", 50, ElementType.Normal);
            Card nonDragon = new FireSpell("Fireball", 20);

            //act
            bool hasEvaded = ((FireElf)fireElve).TryEvadeAttack(nonDragon);

            //assert
            Assert.IsFalse(hasEvaded);
        }
    }
}