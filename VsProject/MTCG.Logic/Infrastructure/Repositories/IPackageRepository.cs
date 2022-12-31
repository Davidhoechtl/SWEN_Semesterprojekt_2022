

namespace MTCG.Logic.Infrastructure.Repositories
{
    using MTCG.Models;

    public interface IPackageRepository
    {
        public bool InsertPackage(Package package);
        public Package GetRandomActivePackage();
        public bool UpdatedPackage(Package package);
    }
}
