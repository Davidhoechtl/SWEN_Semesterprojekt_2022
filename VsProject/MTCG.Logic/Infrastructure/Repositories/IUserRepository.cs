using MTCG.DAL;
using MTCG.Models;

namespace MTCG.Logic.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        bool RegisterUser( string username, string password,  int coins, IUnitOfWork database);
        bool UpdateUser(User user, IUnitOfWork database);
        User GetUserByUsername(string username, IQueryDatabase database);
        User GetUserById(int userId, IQueryDatabase database);

        bool AddCardsToUser(int userId, int[] cardIds, IQueryDatabase database);
        bool RemoveCardsFromUser(int userId, int[] cardIds, IQueryDatabase database);
    }
}
