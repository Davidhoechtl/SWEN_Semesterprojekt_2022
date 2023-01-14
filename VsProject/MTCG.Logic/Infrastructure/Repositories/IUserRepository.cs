using MTCG.DAL;
using MTCG.Models;

namespace MTCG.Logic.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Return only the user opbject without cards, deck, or stats
        /// </summary>
        /// <returns> user overviews </returns>
        List<User> GetAllUsersCore(IQueryDatabase queryDatabase);
        bool RegisterUser( string username, string password,  int coins, int elo, IUnitOfWork database);
        bool UpdateUser(User user, IUnitOfWork database);
        User GetUserByUsername(string username, IQueryDatabase database);
        User GetUserById(int userId, IQueryDatabase database);

        bool AddCardsToUser(int userId, int[] cardIds, IQueryDatabase database);
        bool RemoveCardsFromUser(int userId, int[] cardIds, IQueryDatabase database);
    }
}
