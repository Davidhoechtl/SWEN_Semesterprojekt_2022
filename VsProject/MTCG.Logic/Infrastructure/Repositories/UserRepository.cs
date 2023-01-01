﻿
namespace MTCG.Logic.Infrastructure.Repositories
{
    using MTCG.DAL;
    using MTCG.Models;
    using Npgsql;

    public class UserRepository : IUserRepository
    {
        public UserRepository(IQueryDatabase database, ICardRepository cardRepository)
        {
            this.database = database;
            this.cardRepository = cardRepository;
        }

        public User GetUserByUsername(string username)
        {
            string sqlStatement =
                @"SELECT users.*
                  FROM Users
                  WHERE username = @username";

            User user = database.GetItem<User>(
                sqlStatement,
                ConvertUserFromReader,
                new NpgsqlParameter("username", username)
            );

            user.Stack = cardRepository.GetUserCards(user.Id).ToList();

            return user;
        }

        public User GetUserById(int userId)
        {
            string sqlStatement = "SELECT * FROM Users WHERE user_id = @user_Id";
            User user = database.GetItem<User>(
                sqlStatement,
                ConvertUserFromReader,
                new NpgsqlParameter("user_Id", userId)
            );

            user.Stack = cardRepository.GetUserCards(user.Id).ToList();

            return user;
        }

        public bool RegisterUser(string username, string password)
        {
            string sqlStatement = "INSERT INTO Users (username, password) VALUES (@username, @password)";

            int affectedRows = database.ExecuteNonQuery(
                sqlStatement,
                new NpgsqlParameter("username", username),
                new NpgsqlParameter("password", password)
            );

            return affectedRows != 0;
        }

        public bool UpdateUser(User user)
        {
            string sqlStatement = "UPDATE Users SET username = @username, password = @password, coins = @coins WHERE user_Id = @user_Id";
            int affectedRows = database.ExecuteNonQuery(
                sqlStatement,
                new NpgsqlParameter("user_Id", user.Id),
                new NpgsqlParameter("username", user.Credentials.UserName),
                new NpgsqlParameter("password", user.Credentials.Password),
                new NpgsqlParameter("coins", user.Coins)
            );

            UpdatedUserCards(user);

            return affectedRows != 0;
        }

        public bool AddCardsToUser(int userId, int[] cardIds)
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

        public bool RemoveCardsFromUser(int userId, int[] cardIds)
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
                    Coins = reader.GetInt32(3)
                };
            }

            reader.Close();
            return user;
        }

        private bool UpdatedUserCards(User user)
        {
            string deleteStatement = "DELETE FROM users_cards WHERE user_id = @userId";
            int deleteDeltaRows = database.ExecuteNonQuery(
                deleteStatement,
                new NpgsqlParameter("userId", user.Id)
            );

            string insertStatement = "INSERT INTO users_cards (user_id, card_id, count) VALUES (@userId, @cardId, @count)";
            IEnumerable<IGrouping<int, Card>> cards = user.Stack.GroupBy(x => x.Id);
            foreach(IGrouping<int, Card> pair in cards)
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

        private readonly IQueryDatabase database;
        private readonly ICardRepository cardRepository;
    }
}
