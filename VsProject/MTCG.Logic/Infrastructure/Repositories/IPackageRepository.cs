

namespace MTCG.Logic.Infrastructure.Repositories
{
    using MTCG.DAL;
    using MTCG.Models;

    public interface IPackageRepository
    {
        public bool InsertPackage(Package package, IUnitOfWork database);
        public Package GetRandomActivePackage( IQueryDatabase database );
        public bool UpdatedPackage(Package package, IQueryDatabase database);
    }
}
