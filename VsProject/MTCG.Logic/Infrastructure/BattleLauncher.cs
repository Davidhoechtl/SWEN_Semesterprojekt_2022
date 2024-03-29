﻿using MTCG.Logic.Models;
using MTCG.Models;
using System.Text;

namespace MTCG.Logic.Infrastructure
{
    public class BattleLauncher
    {
        private readonly Random rnd;

        public BattleLauncher(Random rnd)
        {
            this.rnd = rnd;
        }

        public (BattleResult result, string protocol) Launch(User player1, User player2)
        {
            bool validBattleConfiguration = ValidateBattle(player1, player2);
            if (!validBattleConfiguration)
            {
                return (BattleResult.Invalid, string.Empty);
            }

            int roundCount = 1;
            StringBuilder battleProtocol = new();
            battleProtocol.AppendLine($"{player1.Credentials.UserName} battles against {player2.Credentials.UserName}");
            while (player1.Deck.Cards.Count > 0 && player2.Deck.Cards.Count > 0 && roundCount < 100)
            {
                Shuffle(player1.Deck.Cards);
                Shuffle(player2.Deck.Cards);
                Card player1Card = player1.Deck.Cards.Pop();
                Card player2Card = player2.Deck.Cards.Pop();

                battleProtocol.Append($"{roundCount}: {player1Card.Name} ({player1Card.Damage}) battles against {player2Card.Name} ({player2Card.Damage})");
                Card winnersCard = player1Card.BattleAgainst(player2Card);
                battleProtocol.AppendLine($" -> {winnersCard?.Name ?? "draw"}");

                if (player1Card == winnersCard)
                {
                    // plalyer 1 won the round
                    player1.Deck.Cards.Push(winnersCard);
                }
                else if (player2Card == winnersCard)
                {
                    // player 2 won the round
                    player2.Deck.Cards.Push(winnersCard);
                }
                else
                {
                    // Draw
                    player1.Deck.Cards.Push(player1Card);
                    player2.Deck.Cards.Push(player2Card);
                }

                roundCount++;
            }

            if (player1.Deck.Cards.Count == 0)
            {
                battleProtocol.AppendLine($"{player2.Credentials.UserName} won the battle");
                return (BattleResult.Lose, battleProtocol.ToString());
            }
            else if (player2.Deck.Cards.Count == 0)
            {
                battleProtocol.AppendLine($"{player1.Credentials.UserName} won the battle");
                return (BattleResult.Won, battleProtocol.ToString());
            }
            else
            {
                battleProtocol.AppendLine($"Battle ended in a draw");
                return (BattleResult.Draw, battleProtocol.ToString());
            }
        }

        private bool ValidateBattle(User player1, User player2)
        {
            bool player1DeckValid = player1.Deck != null && player1.Deck.Cards.Count != 0;
            bool player2DeckValid = player2.Deck != null && player2.Deck.Cards.Count != 0;
            bool sameCardCount = player1.Deck?.Cards.Count == player2.Deck?.Cards.Count;

            if (player1DeckValid && player2DeckValid && sameCardCount)
            {
                return true;
            }

            return false;
        }

        private void Shuffle(Stack<Card> stack)
        {
            var values = stack.ToArray();
            stack.Clear();
            foreach (var value in values.OrderBy(x => rnd.Next()))
                stack.Push(value);
        }
    }
}
