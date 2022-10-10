using MTCG.Models;

namespace MTCG.Logic.Infrastructure.Repositories
{
    public class MockUserRepository : IUserRepository
    {
        private static readonly IEnumerable<User> registeredUsers = new List<User>()
        {
            new User()
            {
                // Passwörter müssen verschlüsselt in der Datenbank stehen!
                Credentials = new UserCredentials("TestUser", "Test1234"),
                Coins = 20
            },
            new User()
            {
                // Passwörter müssen verschlüsselt in der Datenbank stehen!
                Credentials = new UserCredentials("TestUser2", "Test4321"),
                Coins = 20
            }
        };

        public User GetUserByUsername(string username)
        {
            return registeredUsers
                .FirstOrDefault(user => user.Credentials.UserName.Equals(username, StringComparison.Ordinal));
        }
    }
}
