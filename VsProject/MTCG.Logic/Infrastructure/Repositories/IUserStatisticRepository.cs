
namespace MTCG.Logic.Infrastructure.Repositories
{
    using MTCG.DAL;
    using MTCG.Logic.Models;

    public interface IUserStatisticRepository
    {
        public IEnumerable<UserStatistic> GetAllUserStats(IQueryDatabase queryDatabase);
        UserStatistic GetStatisticByUserId(int userId, IQueryDatabase queryDatabase);
        bool UpdateUserStatistic(UserStatistic userStatistic, IUnitOfWork unitOfWork);
    }
}
