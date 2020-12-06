using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Npgsql;
using SWE1_MTCG.Cards.Monster;
using SWE1_MTCG.Cards.Spells;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Interfaces;

namespace SWE1_MTCG.Cards
{
    public class Package : ICardCollection
    {

        #region fields
        #endregion

        #region properties
        public PackageType PackageType { get; set; }
        public List<Card> Cards { get; }
        #endregion

        #region constructor

        public Package()
        {
            Cards= new List<Card>();
            PackageType = PackageType.Random;
            GenerateCards();
        }

        public Package(IEnumerable<Card> cards)
        {
            Cards = cards.ToList();
            PackageType = PackageType.Basic;
        }
        #endregion

        #region private methods

        private void GenerateCards()
        {
            //TODO add 5 random cards from card-pool
            Card fireDragon= new Dragon("Fire Dragon", 70, ElementType.Fire);
            Card goldenKnight= new Knight("Golden Knight", 45, ElementType.Normal);
            Card gandalf= new Wizard("Gandalf", 30, ElementType.Water);
            Card cascade = new WaterSpell("Cascade", 35);
            Card theodor= new Orc("Theodor", 50, ElementType.Normal);

            Cards.AddRange(new [] {fireDragon, goldenKnight, gandalf, cascade, theodor});
        }
        #endregion

        #region public methods

        #endregion


        public IEnumerable<Card> GetAllCards()
        {
            return Cards;
        }
    }
}
