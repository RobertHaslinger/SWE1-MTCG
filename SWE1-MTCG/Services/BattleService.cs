using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Battle;
using SWE1_MTCG.Cards;
using SWE1_MTCG.Cards.Monster;
using SWE1_MTCG.Enums;
using SWE1_MTCG.Interfaces;

namespace SWE1_MTCG.Services
{
    public class BattleService : IBattleService
    {
        private IElementService _elementService;

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
        private void ProcessRound(ref BattleDeck battleDeckP1, ref BattleDeck battleDeckP2)
        {
            Card cardP1 = battleDeckP1.GetRandomCard();
            Card cardP2 = battleDeckP2.GetRandomCard();

            BattleResult roundWinner = cardP1 switch
            {
                ISpell _ when cardP2 is ISpell => FightSpellVsSpell(cardP1, cardP2),
                IMonster _ when cardP2 is ISpell => FightMonsterVsSpell(cardP1, cardP2),
                ISpell _ when cardP2 is IMonster => FightMonsterVsSpell(cardP2, cardP1),
                _ => FightMonsterVsMonster(cardP1, cardP2)
            };

            if (roundWinner == BattleResult.Player1Wins)
            {
                battleDeckP2.RemoveCard(cardP2);
                battleDeckP1.AddCard(cardP2);
            }
            else
            {
                battleDeckP1.RemoveCard(cardP1);
                battleDeckP2.AddCard(cardP1);
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

        private BattleResult FightMonsterVsSpell(Card cardP1, Card cardP2)
        {
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
            if (cardP1.Damage.CompareTo(cardP2.Damage) > 0) return BattleResult.Player1Wins;
            return cardP1.Damage.CompareTo(cardP2.Damage) == 0 ? BattleResult.Draw : BattleResult.Player2Wins;
        }

        private BattleResult CalculateWinnerByDamageAndElement(Card cardP1, Card cardP2)
        {
            double effectiveness = _elementService.CompareElement(cardP1.Element, cardP2.Element);

            if (cardP1.Damage * effectiveness.CompareTo(cardP2.Damage) > 0) return BattleResult.Player1Wins;
            return cardP1.Damage * effectiveness.CompareTo(cardP2.Damage) == 0 ? BattleResult.Draw : BattleResult.Player2Wins;
        }

        #endregion

        public void CalculateAndApplyMmr(User winner, User loser)
        {
            throw new NotImplementedException();
        }

        public BattleResult StartBattle(User player1, User player2)
        {
            int rounds = 0;
            BattleDeck battleDeckP1 = player1.Deck.GetBattleDeck();
            BattleDeck battleDeckP2 = player2.Deck.GetBattleDeck();
            while (battleDeckP1.HasCardsLeft() && battleDeckP2.HasCardsLeft() && rounds < 100)
            {
                ProcessRound(ref battleDeckP1, ref battleDeckP2);
                rounds++;
            }

            if (battleDeckP1.HasCardsLeft() && battleDeckP2.HasCardsLeft()) return BattleResult.Draw;
            return battleDeckP1.HasCardsLeft() ? BattleResult.Player1Wins : BattleResult.Player2Wins;
        }
    }
}
