
namespace MTCG.Tests
{
    internal class BattleValidationTests
    {
        [Test]
        public void Test_Invalid_OneUserHasNoDeck()
        {
            BattleLauncher battleLauncher = new BattleLauncher();
            ICardRepository cardRepository = new MockCardRepository();
            IEnumerable<Card> cards = cardRepository.GetAllAvailableCards();

            User user1 = new User { Deck = new Deck() { Cards = new Stack<Card>(cards.Take(3)) } };
            User user2 = new User { Deck = null };

            BattleResult result = battleLauncher.Launch(user1, user2);

            Assert.That(result == BattleResult.Invalid);
        }

        [Test]
        public void Test_Invalid_UserHaveDifferentAmountsOfCards()
        {
            BattleLauncher battleLauncher = new BattleLauncher();
            ICardRepository cardRepository = new MockCardRepository();
            IEnumerable<Card> cards = cardRepository.GetAllAvailableCards();

            User user1 = new User { Deck = new Deck() { Cards = new Stack<Card>(cards.Take(3)) } };
            User user2 = new User { Deck = new Deck() { Cards = new Stack<Card>(cards.Take(5)) } };

            BattleResult result = battleLauncher.Launch(user1, user2);

            Assert.That(result == BattleResult.Invalid);
        }

        [Test]
        public void Test_Invalid_OneUserHasNoCards()
        {
            BattleLauncher battleLauncher = new BattleLauncher();
            ICardRepository cardRepository = new MockCardRepository();
            IEnumerable<Card> cards = cardRepository.GetAllAvailableCards();

            User user1 = new User { Deck = new Deck() { Cards = new Stack<Card>(cards.Take(3)) } };
            User user2 = new User { Deck = new Deck() { Cards = new Stack<Card>() } };

            BattleResult result = battleLauncher.Launch(user1, user2);

            Assert.That(result == BattleResult.Invalid);
        }

        [Test]
        public void Test_Valid()
        {
            BattleLauncher battleLauncher = new BattleLauncher();
            ICardRepository cardRepository = new MockCardRepository();
            IEnumerable<Card> cards = cardRepository.GetAllAvailableCards();

            User user1 = new User { Deck = new Deck() { Cards = new Stack<Card>(cards.Take(3)) } };
            User user2 = new User { Deck = new Deck() { Cards = new Stack<Card>(cards.Take(3)) } };

            BattleResult result = battleLauncher.Launch(user1, user2);

            Assert.That(result != BattleResult.Invalid);
        }
    }
}
