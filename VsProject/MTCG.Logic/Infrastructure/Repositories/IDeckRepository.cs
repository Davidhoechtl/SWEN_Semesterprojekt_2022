
namespace MTCG.Logic.Infrastructure.Repositories
{
    using MTCG.DAL;
    using MTCG.Models;

    public interface IDeckRepository
    {
        Deck GetUsersDeck(int userId, IQueryDatabase queryDatabase);
        bool UpdateUsersDeck(Deck deck, IUnitOfWork unitOfWork);
    }
}
