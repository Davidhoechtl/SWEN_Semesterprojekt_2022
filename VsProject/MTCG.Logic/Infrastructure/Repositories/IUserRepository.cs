using MTCG.Models;

namespace MTCG.Logic.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        User GetUserByUsername(string username);
    }
}
