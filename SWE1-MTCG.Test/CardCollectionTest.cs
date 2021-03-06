﻿using System.Collections;
using System.Reflection.Metadata.Ecma335;
using NUnit.Framework;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Cards.Monster;
using SWE1_MTCG.Cards.Spells;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Interfaces;

namespace SWE1_MTCG.Test
{
    [TestFixture]
    public class CardCollectionTest
    {

        [Test]
        public void Test_ICardCollectionShouldReturnEmptyIEnumerableIfThereAreNoCards()
        {
            //arrange
            ICardCollection cardStack = new CardStack();

            //act
            IEnumerable cards = cardStack.GetAllCards();

            //assert
            Assert.IsEmpty(cards);
        }

        [Test]
        public void Test_PackageShouldHaveCardsAfterInitialized()
        {
            //arrange
            ICardCollection package= new Package();

            //act
            IEnumerable cards = package.GetAllCards();

            //assert
            Assert.IsNotEmpty(cards);
        }

        [Test]
        public void Test_DeckShouldOnlyGetRandomCardThatIsInDeck()
        {
            //arrange
            CardStack deck = new Deck();
            Card dragon = new Dragon("Great Dragon", 40, ElementType.Normal);
            Card fireSpell = new FireSpell("Fireball", 60);


            //act
            deck.AddCard(dragon);
            deck.AddCard(fireSpell);
            Card randomCard = ((Deck)deck).GetRandomCard();
            bool isInDeck = ((Deck)deck).IsInDeck(randomCard);

            //assert
            Assert.IsTrue(isInDeck);
        }

        [Test]
        public void Test_DeckShouldNotAddCardWhenThereAreAlreadyTwoCopies()
        {
            //arrange
            CardStack deck = new Deck();
            Card dragon = new Dragon("Great Dragon", 40, ElementType.Normal);

            //act
            bool cardAdded;
            deck.AddCard(dragon);
            deck.AddCard(dragon);
            cardAdded = deck.AddCard(dragon);

            //assert
            Assert.IsFalse(cardAdded);
        }

        [Test]
        public void Test_DeckShouldNotAddCardWhenThereAreAlreadyFourCards()
        {
            //arrange
            CardStack deck = new Deck();
            Card dragon = new Dragon("Great Dragon", 40, ElementType.Normal);
            Card fireDragon = new Dragon("Small Fire Dragon", 20, ElementType.Fire);
            Card tentakel= new Kraken("Tentakel", 30, ElementType.Water);

            //act
            bool cardAdded;
            deck.AddCard(dragon);
            deck.AddCard(dragon);
            deck.AddCard(fireDragon);
            deck.AddCard(tentakel);
            cardAdded = deck.AddCard(tentakel);

            //assert
            Assert.IsFalse(cardAdded);
        }
    }
}