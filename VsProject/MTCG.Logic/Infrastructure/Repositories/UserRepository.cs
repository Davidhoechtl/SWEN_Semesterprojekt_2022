
namespace MTCG.Logic.Infrastructure.Repositories
{
    using MTCG.DAL;
    using MTCG.Models;
    using Npgsql;
    using System.Collections.Generic;

    public class UserRepository : IUserRepository
    {
        public UserRepository(ICardRepository cardRepository, IDeckRepository deckRepository, IUserStatisticRepository statisticRepository)
        {
            this.cardRepository = cardRepository;
            this.deckRepository = deckRepository;
            this.statisticRepository = statisticRepository;
        }

        public List<User> GetAllUsersCore(IQueryDatabase queryDatabase)
        {
            string sqlStatement = @"SELECT users.* FROM users";

            return queryDatabase.GetItems(
                sqlStatement,
                reader =>
                {
                    List<User> users = new();
                    while (reader.Read())
                    {
                        users.Add(ConvertUserFromReader(reader));
                    }
                    reader.Close();
                    return users;
                }
            );
        }

        public User GetUserByUsername(string username, IQueryDatabase database)
        {
            string sqlStatement =
                @"SELECT users.*
                  FROM Users
                  WHERE username = @username";

            User user = database.GetItem<User>(
                sqlStatement,
                reader =>
                {
                    User user = ConvertUserFromReader(reader);
                    reader.Close();
                    return user;
                },
                new NpgsqlParameter("username", username)
            );

            if (user != null)
            {
                user.Cards = cardRepository.GetUserCards(user.Id, database).ToList();
                user.Deck = deckRepository.GetUsersDeck(user.Id, database);
                user.Statistic = statisticRepository.GetStatisticByUserId(user.Id, database);
            }

            return user;
        }

        public User GetUserById(int userId, IQueryDatabase database)
        {
            string sqlStatement = "SELECT * FROM Users WHERE user_id = @user_Id";
            User user = database.GetItem<User>(
                sqlStatement,
                reader =>
                {
                    User user = ConvertUserFromReader(reader);
                    reader.Close();
                    return user;
                },
                new NpgsqlParameter("user_Id", userId)
            );

            if (user != null)
            {
                user.Cards = cardRepository.GetUserCards(user.Id, database).ToList();
                user.Deck = deckRepository.GetUsersDeck(user.Id, database);
                user.Statistic = statisticRepository.GetStatisticByUserId(user.Id, database);
            }

            return user;
        }

        public bool RegisterUser(string username, string password, int coins, int elo, IUnitOfWork database)
        {
            string sqlStatement = "INSERT INTO Users (username, password, coins, elo) VALUES (@username, @password, @coins, @elo) RETURNING user_id";

            int? user_id = database.InsertAndGetLastIdentity(
                sqlStatement,
                new NpgsqlParameter("username", username),
                new NpgsqlParameter("password", password),
                new NpgsqlParameter("coins", coins),
                new NpgsqlParameter("elo", elo)
            );

            if (user_id.HasValue)
            {
                sqlStatement = "INSERT INTO decks (user_id) VALUES (@userId)";
                int affectedRows = database.ExecuteNonQuery(
                    sqlStatement,
                    new NpgsqlParameter("userId", user_id.Value)
                );

                sqlStatement = "INSERT INTO users_stats (user_Id, username, coins_spent, battles_played, wins, win_rate) VALUES (@userId, @username, 0, 0, 0, 0)";
                int affectedRows2 = database.ExecuteNonQuery(
                    sqlStatement,
                    new NpgsqlParameter("userId", user_id.Value),
                    new NpgsqlParameter("username", username)
                );

                return affectedRows != 0 && affectedRows2 != 0;
            }

            return false;
        }

        public bool UpdateUser(User user, IUnitOfWork database)
        {
            string sqlStatement = "UPDATE Users SET username = @username, password = @password, coins = @coins, elo = @elo WHERE user_Id = @user_Id";
            int affectedRows = database.ExecuteNonQuery(
                sqlStatement,
                new NpgsqlParameter("user_Id", user.Id),
                new NpgsqlParameter("username", user.Credentials.UserName),
                new NpgsqlParameter("password", user.Credentials.Password),
                new NpgsqlParameter("coins", user.Coins),
                new NpgsqlParameter("elo", user.ELO)
            );

            UpdatedUserCards(user, database);
            deckRepository.UpdateUsersDeck(user.Deck, database);
            statisticRepository.UpdateUserStatistic(user.Statistic, database);

            return affectedRows != 0;
        }

        public bool AddCardsToUser(int userId, int[] cardIds, IQueryDatabase database)
        {
            string sqlStatement = "INSERT INTO users_cards (user_id, card_id) VALUES (@userId, @cardId)";

            int affectedRows = 0;
            foreach (int cardId in cardIds)
            {
                int deltaRows = database.ExecuteNonQuery(
                    sqlStatement,
                    new NpgsqlParameter("userId", userId),
                    new NpgsqlParameter("cardId", cardId)
                );

                affectedRows += deltaRows;
            }

            return affectedRows != 0;
        }

        public bool RemoveCardsFromUser(int userId, int[] cardIds, IQueryDatabase database)
        {
            string sqlStatement = "DELETE users_cards WHERE user_id = @userId AND card_id = @cardId";

            int affectedRows = 0;
            foreach (int cardId in cardIds)
            {
                int deltaRows = database.ExecuteNonQuery(
                    sqlStatement,
                    new NpgsqlParameter("userId", userId),
                    new NpgsqlParameter("cardId", cardId)
                );

                affectedRows += deltaRows;
            }

            return affectedRows != 0;
        }

        private User ConvertUserFromReader(NpgsqlDataReader reader)
        {
            User user = null;

            if (reader.IsOnRow)
            {
                user = new User()
                {
                    Id = reader.GetInt32(0),
                    Credentials = new UserCredentials()
                    {
                        UserName = reader.GetString(1),
                        Password = reader.GetString(2)
                    },
                    Coins = reader.GetInt32(3),
                    ELO = reader.GetInt32(reader.GetOrdinal("elo"))
                };
            }

            return user;
        }

        private bool UpdatedUserCards(User user, IQueryDatabase database)
        {
            string deleteStatement = "DELETE FROM users_cards WHERE user_id = @userId";
            int deleteDeltaRows = database.ExecuteNonQuery(
                deleteStatement,
                new NpgsqlParameter("userId", user.Id)
            );

            string insertStatement = "INSERT INTO users_cards (user_id, card_id, count) VALUES (@userId, @cardId, @count)";
            IEnumerable<IGrouping<int, Card>> cards = user.Cards.GroupBy(x => x.Id);
            foreach (IGrouping<int, Card> pair in cards)
            {
                int insertDeltaRow = database.ExecuteNonQuery(
                    insertStatement,
                    new NpgsqlParameter("userId", user.Id),
                    new NpgsqlParameter("cardId", pair.Key),
                    new NpgsqlParameter("count", pair.Count())
                );
            }

            return true;
        }

        private readonly ICardRepository cardRepository;
        private readonly IDeckRepository deckRepository;
        private readonly IUserStatisticRepository statisticRepository;
    }
}
