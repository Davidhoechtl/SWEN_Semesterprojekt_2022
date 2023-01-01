﻿using MTCG.DAL;
using MTCG.Models;

namespace MTCG.Logic.Infrastructure.Repositories.MockUps
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

        public bool AddCardsToUser(int userId, int[] cardIds, IQueryDatabase database)
        {
            throw new NotImplementedException();
        }

        public User GetUserById(int userId, IQueryDatabase database)
        {
            throw new NotImplementedException();
        }

        public User GetUserByUsername(string username, IQueryDatabase database)
        {
            return registeredUsers
                .FirstOrDefault(user => user.Credentials.UserName.Equals(username, StringComparison.Ordinal));
        }

        public bool RemoveCardsFromUser(int userId, int[] cardIds, IQueryDatabase database)
        {
            throw new NotImplementedException();
        }

        public bool RegisterUser(string username, string password, IQueryDatabase database)
        {
            return true;
        }

        public bool UpdateUser(User user, IUnitOfWork database)
        {
            return true;
        }
    }
}
