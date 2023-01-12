
namespace MTCG.Tests
{
    using MTCG.DAL;
    using MTCG.Logic.Infrastructure.Repositories.MockUps;
    using Npgsql;

    internal class BattleValidationTests
    {
        private Random rnd;

        [SetUp]
        public void Setup()
        {
            rnd = new Random();
        }

        [Test]
        public void Test_Invalid_OneUserHasNoDeck()
        {
            BattleLauncher battleLauncher = new BattleLauncher(rnd);
            ICardRepository cardRepository = new MockCardRepository();
            IEnumerable<Card> cards = cardRepository.GetAllAvailableCards(null);

            User user1 = new User { Deck = new Deck() { Cards = new Stack<Card>(cards.Take(3)) } };
            User user2 = new User { Deck = null };

            (BattleResult result, string protocol) battleResult = battleLauncher.Launch(user1, user2);

            Assert.That(battleResult.result == BattleResult.Invalid);
        }

        [Test]
        public void Test_Invalid_UserHaveDifferentAmountsOfCards()
        {
            BattleLauncher battleLauncher = new BattleLauncher(rnd);
            ICardRepository cardRepository = new MockCardRepository();
            IEnumerable<Card> cards = cardRepository.GetAllAvailableCards(null);

            User user1 = new User { Deck = new Deck() { Cards = new Stack<Card>(cards.Take(3)) } };
            User user2 = new User { Deck = new Deck() { Cards = new Stack<Card>(cards.Take(5)) } };

            (BattleResult result, string protocol) battleResult = battleLauncher.Launch(user1, user2);

            Assert.That(battleResult.result == BattleResult.Invalid);
        }

        [Test]
        public void Test_Invalid_OneUserHasNoCards()
        {
            BattleLauncher battleLauncher = new BattleLauncher(rnd);
            ICardRepository cardRepository = new MockCardRepository();
            IEnumerable<Card> cards = cardRepository.GetAllAvailableCards(null);

            User user1 = new User { Deck = new Deck() { Cards = new Stack<Card>(cards.Take(3)) } };
            User user2 = new User { Deck = new Deck() { Cards = new Stack<Card>() } };

            (BattleResult result, string protocol) battleResult = battleLauncher.Launch(user1, user2);

            Assert.That(battleResult.result == BattleResult.Invalid);
        }

        [Test]
        public void Test_Valid()
        {
            BattleLauncher battleLauncher = new BattleLauncher(rnd);
            ICardRepository cardRepository = new MockCardRepository();
            IEnumerable<Card> cards = cardRepository.GetAllAvailableCards(null);

            User user1 = new User { Deck = new Deck() { Cards = new Stack<Card>(cards.Take(3)) } };
            user1.Credentials = new UserCredentials() { UserName = "user1", Password= "password" };
            User user2 = new User { Deck = new Deck() { Cards = new Stack<Card>(cards.Take(3)) } };
            user2.Credentials = new UserCredentials() { UserName = "user2", Password = "password" };

            (BattleResult result, string protocol) battleResult = battleLauncher.Launch(user1, user2);

            Assert.That(battleResult.result != BattleResult.Invalid);
        }
    }
}
