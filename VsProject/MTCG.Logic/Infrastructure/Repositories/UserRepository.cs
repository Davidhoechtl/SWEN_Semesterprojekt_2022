
namespace MTCG.Logic.Infrastructure.Repositories
{
    using MTCG.DAL;
    using MTCG.Models;
    using Npgsql;

    public class UserRepository : IUserRepository
    {
        public UserRepository(IQueryDatabase database)
        {
            this.database = database;
        }

        public User GetUserByUsername(string username)
        {
            string sqlStatement = "SELECT user_Id, username, password FROM Users WHERE username = @username";
            User user = database.GetItem<User>(
                sqlStatement,
                ConvertUserFromReader,
                new NpgsqlParameter("username", username)
            );

            return user;
        }

        public User GetUserById(int userId)
        {
            string sqlStatement = "SELECT user_Id, username, password FROM Users WHERE user_id = @user_Id";
            User user = database.GetItem<User>(
                sqlStatement,
                ConvertUserFromReader,
                new NpgsqlParameter("user_Id", userId)
            );

            return user;
        }

        public bool SaveUser(string username, string password)
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
            string sqlStatement = "UPDATE Users SET username = @username, password = @password WHERE user_Id = @user_Id";

            int affectedRows = database.ExecuteNonQuery(
                sqlStatement,
                new NpgsqlParameter("user_Id", user.Id),
                new NpgsqlParameter("username", user.Credentials.UserName),
                new NpgsqlParameter("password", user.Credentials.Password)
            );

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
                    }
                };
            }

            reader.Close();
            return user;
        }

        private readonly IQueryDatabase database;
    }
}
