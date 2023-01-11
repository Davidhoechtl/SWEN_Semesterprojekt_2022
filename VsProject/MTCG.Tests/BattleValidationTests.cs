
namespace MTCG.Tests
{
    using MTCG.DAL;
    using MTCG.Logic.Infrastructure.Repositories.MockUps;
    using Npgsql;

    internal class BattleValidationTests
    {
        private Random rnd;
        private IQueryDatabase mockDatabase;

        [SetUp]
        public void Setup()
        {
            NpgsqlConnection mockConnection = new NpgsqlConnection();
            mockDatabase = new NpgSqlQueryDatabase(mockConnection);
            rnd = new Random();
        }

        [Test]
        public void Test_Invalid_OneUserHasNoDeck()
        {
            BattleLauncher battleLauncher = new BattleLauncher(rnd);
            ICardRepository cardRepository = new MockCardRepository();
            IEnumerable<Card> cards = cardRepository.GetAllAvailableCards(mockDatabase);

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
            IEnumerable<Card> cards = cardRepository.GetAllAvailableCards(mockDatabase);

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
            IEnumerable<Card> cards = cardRepository.GetAllAvailableCards(mockDatabase);

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
            IEnumerable<Card> cards = cardRepository.GetAllAvailableCards(mockDatabase);

            User user1 = new User { Deck = new Deck() { Cards = new Stack<Card>(cards.Take(3)) } };
            User user2 = new User { Deck = new Deck() { Cards = new Stack<Card>(cards.Take(3)) } };

            (BattleResult result, string protocol) battleResult = battleLauncher.Launch(user1, user2);

            Assert.That(battleResult.result == BattleResult.Invalid);
        }
    }
}
