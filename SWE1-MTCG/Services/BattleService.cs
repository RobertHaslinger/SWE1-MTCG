using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Battle;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Cards.Monster;
using SWE1_MTCG.Client;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Interfaces;

namespace SWE1_MTCG.Services
{
    public class BattleService : IBattleService
    {
        private IElementService _elementService;
        private double _accuracy = 1.0;

        public BattleService(IElementService elementService)
        {
            _elementService = elementService;
        }

        #region private methods

        /// <summary>
        /// Process a round in a battle with two given BattleDeck objects.
        /// Round Winner calculation uses Discards and when clause introduced in C#7.0 and switch expression introduced in C#8.0.
        /// Ref when clause: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/switch#the-case-statement-and-the-when-clause
        /// Ref Discards: https://docs.microsoft.com/en-us/dotnet/csharp/discards
        /// </summary>
        /// <param name="battleDeckP1"></param>
        /// <param name="battleDeckP2"></param>
        private void ProcessRound(string player1Name, string player2Name, ref BattleDeck battleDeckP1, ref BattleDeck battleDeckP2, ref BattleLog log)
        {
            Card cardP1 = battleDeckP1.GetRandomCard();
            Card cardP2 = battleDeckP2.GetRandomCard();
            //generate the accuracy of the next attack (max 1.7, min 0.3
            _accuracy = new Random().NextDouble() * (1.7 - 0.3) + 0.3;

            BattleResult roundWinner = cardP1 switch
            {
                ISpell _ when cardP2 is ISpell => FightSpellVsSpell(cardP1, cardP2),
                IMonster _ when cardP2 is ISpell => FightMonsterVsSpell(cardP1, cardP2),
                ISpell _ when cardP2 is IMonster => FightMonsterVsSpell(cardP2, cardP1, true),
                _ => FightMonsterVsMonster(cardP1, cardP2)
            };

            if (roundWinner == BattleResult.Player1Wins)
            {
                battleDeckP2.RemoveCard(cardP2);
                battleDeckP1.AddCard(cardP2);
                log.Rounds.Add(
                    $"{cardP1.Name} ({cardP1.GetType().Name}, {player1Name}) won with damage {cardP1.Damage} and element {Enum.GetName(typeof(ElementType), cardP1.Element)} " +
                    $"against {cardP2.Name} ({cardP2.GetType().Name}, {player2Name}) with damage {cardP2.Damage} and element {Enum.GetName(typeof(ElementType), cardP2.Element)}. " +
                    $"The accuracy of the attack was {Math.Round(_accuracy, 3)}");
            }
            else
            {
                battleDeckP1.RemoveCard(cardP1);
                battleDeckP2.AddCard(cardP1);
                log.Rounds.Add(
                    $"{cardP1.Name} ({cardP1.GetType().Name}, {player1Name}) lost with damage {cardP1.Damage} and element {Enum.GetName(typeof(ElementType), cardP1.Element)} " +
                    $"against {cardP2.Name} ({cardP2.GetType().Name}, {player2Name}) with damage {cardP2.Damage} and element {Enum.GetName(typeof(ElementType), cardP2.Element)}. " +
                    $"The accuracy of the attack was {Math.Round(_accuracy, 3)}");
            }
        }

        private BattleResult FightMonsterVsMonster(Card cardP1, Card cardP2)
        {
            return cardP1 switch
            {
                Wizard wizard when wizard.TryControlOrc(cardP2) => BattleResult.Player1Wins,
                Goblin goblin when goblin.TryActScared(cardP2) => BattleResult.Draw,
                Dragon dragon when cardP2 is FireElf elf && elf.TryEvadeAttack(dragon) => BattleResult.Draw,
                _ => CalculateWinnerByDamage(cardP1, cardP2)
            };
        }

        private BattleResult FightMonsterVsSpell(Card cardP1, Card cardP2, bool inverse=false)
        {
            if (inverse)
            {
                var temp = cardP2;
                cardP2 = cardP1;
                cardP1 = temp;
            }

            return cardP1 switch
            {
                Knight knight when knight.TryDrown(cardP2) => BattleResult.Player2Wins,
                Kraken kraken when kraken.TryResistSpell(cardP2) => BattleResult.Player1Wins,
                _ => CalculateWinnerByDamageAndElement(cardP1, cardP2)
            };
        }

        private BattleResult FightSpellVsSpell(Card cardP1, Card cardP2)
        {
            return CalculateWinnerByDamageAndElement(cardP1, cardP2);
        }

        private BattleResult CalculateWinnerByDamage(Card cardP1, Card cardP2)
        {
            if ((cardP1.Damage * _accuracy).CompareTo(cardP2.Damage) > 0) return BattleResult.Player1Wins;
            return (cardP1.Damage * _accuracy).CompareTo(cardP2.Damage) == 0 ? BattleResult.Draw : BattleResult.Player2Wins;
        }

        private BattleResult CalculateWinnerByDamageAndElement(Card cardP1, Card cardP2)
        {
            double effectiveness = _elementService.CompareElement(cardP1.Element, cardP2.Element) * _accuracy;


            if ((cardP1.Damage * effectiveness).CompareTo(cardP2.Damage) > 0) return BattleResult.Player1Wins;
            return (cardP1.Damage * effectiveness).CompareTo(cardP2.Damage) == 0 ? BattleResult.Draw : BattleResult.Player2Wins;
        }

        #endregion

        public KeyValuePair<BattleResult, BattleLog> StartBattle(User player1, User player2)
        {
            int rounds = 0;
            BattleDeck battleDeckP1 = player1.Deck.GetBattleDeck();
            BattleDeck battleDeckP2 = player2.Deck.GetBattleDeck();
            BattleLog log= new BattleLog();
            while (battleDeckP1.HasCardsLeft() && battleDeckP2.HasCardsLeft() && rounds < 100)
            {
                ProcessRound(player1.Username, player2.Username, ref battleDeckP1, ref battleDeckP2, ref log);
                rounds++;
            }

            if (battleDeckP1.HasCardsLeft() && battleDeckP2.HasCardsLeft())
            {
                log.Draw = true;
                return new KeyValuePair<BattleResult, BattleLog>(BattleResult.Draw, log);
            }

            if (battleDeckP1.HasCardsLeft())
            {
                log.Winner = player1.Username;
                log.Loser = player2.Username;
                return new KeyValuePair<BattleResult, BattleLog>(BattleResult.Player1Wins, log);
            }
            log.Winner = player2.Username;
            log.Loser = player1.Username;
            return new KeyValuePair<BattleResult, BattleLog>(BattleResult.Player2Wins, log);
        }
    }
}
