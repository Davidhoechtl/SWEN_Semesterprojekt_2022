
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
                reader =>
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
                },
                new NpgsqlParameter("username", username)
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
                new NpgsqlParameter("user_Id", user.Id)
            );

            return affectedRows != 0;
        }
     
        private readonly IQueryDatabase database;
    }
}
