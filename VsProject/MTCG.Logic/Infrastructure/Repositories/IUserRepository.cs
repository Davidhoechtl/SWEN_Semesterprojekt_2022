using MTCG.Models;

namespace MTCG.Logic.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        bool RegisterUser( string username, string password );
        bool UpdateUser(User user);
        User GetUserByUsername(string username);
        User GetUserById(int userId);

        bool AddCardsToUser(int userId, int[] cardIds);
        bool RemoveCardsFromUser(int userId, int[] cardIds);
    }
}
