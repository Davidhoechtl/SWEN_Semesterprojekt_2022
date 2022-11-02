using MTCG.Models;

namespace MTCG.Logic.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        bool SaveUser( string username, string password );
        bool UpdateUser(User user);
        User GetUserByUsername(string username);
    }
}
