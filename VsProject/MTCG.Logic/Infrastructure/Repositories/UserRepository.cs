
namespace MTCG.Logic.Infrastructure.Repositories
{
    using MTCG.DAL;
    using MTCG.Models;
    using Npgsql;

    public class UserRepository : IUserRepository
    {
        public UserRepository(IDatabase database)
        {
            this.database = database;
        }

        public User GetUserByUsername(string username)
        {
            User user = new();
            string sqlStatement = "SELECT username, password FROM Users WHERE username = '{" + username +"}'";
            NpgsqlDataReader reader = database.GetNpgsqlDataReader(sqlStatement);
            while(reader.Read())
            {
                string name = reader[0].ToString();
                string password = reader[1].ToString();
                user.Credentials = new UserCredentials(name, password);
                reader.Close();
                return user;
            }

            reader.Close();
            return null;
        }

        public bool SaveUser(string username, string password)
        {
            string sqlStatement = "INSERT INTO Users (username, password) VALUES ('{"+username+"}','{"+password+"}')";
            NpgsqlCommand cmd = database.GetNpgsqlCommand(sqlStatement);
            cmd.ExecuteNonQuery();
            return true;
        }

        public bool UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
     
        private readonly IDatabase database;
    }
}
