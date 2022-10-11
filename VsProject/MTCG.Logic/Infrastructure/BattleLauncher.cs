using MTCG.Logic.Models;
using MTCG.Models;

namespace MTCG.Logic.Infrastructure
{
    public class BattleLauncher
    {
        public BattleResult Launch(User player1, User player2)
        {
            bool validBattleConfiguration = ValidateBattle(player1, player2);
            if (!validBattleConfiguration)
            {
                return BattleResult.Invalid;
            }

            int roundCount = 1;
            while (player1.Deck.Cards.Count > 0 && player2.Deck.Cards.Count > 0 && roundCount < 100)
            {
                Card player1Card = player1.Deck.Cards.Pop();
                Card player2Card = player2.Deck.Cards.Pop();

                Card winnersCard = player1Card.BattleAgainst(player2Card);

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
                return BattleResult.Lose;
            }
            else if (player2.Deck.Cards.Count == 0)
            {
                return BattleResult.Won;
            }
            else
            {
                return BattleResult.Draw;
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
    }
}
