
namespace MonsterTradingCardGame_Hoechtl.Handler
{
    using MonsterTradingCardGame_Hoechtl.Handler.HttpAttributes;
    using MonsterTradingCardGame_Hoechtl.Models;
    using MTCG.DAL;
    using MTCG.Infrastructure;
    using MTCG.Logic.Infrastructure.Repositories;
    using MTCG.Logic.Models.RequestContexts;
    using MTCG.Models;

    internal class PackageModule : IHandler
    {
        public string ModuleName => "Package";

        public PackageModule(
            IPackageRepository packageRepository,
            IUserRepository userRepository,
            IQueryDatabase queryDatabase,
            PackageFactory packageFactory,
            UnitOfWorkFactory unitOfWorkFactory,
            Random rnd)
        {
            this.packageRepository = packageRepository;
            this.userRepository = userRepository;
            this.queryDatabase = queryDatabase;
            this.packageFactory = packageFactory;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.rnd = rnd;
        }

        [Post]
        public HttpResponse CreateNewPackage(SessionContext context, CreatePackageContext createPackageContext)
        {
            if (context.SessionKey.Permission != Permission.Admin)
            {
                return HttpResponse.GetUnauthorizedResponse();
            }

            Package package = packageFactory.CreatePackage(
                rnd.Next(createPackageContext.MinItems,
                createPackageContext.MaxItems + 1),
                createPackageContext.Price
            );

            bool success = false;
            using (IUnitOfWork unitOfWork = unitOfWorkFactory.CreateAndBeginTransaction())
            {
                success = packageRepository.InsertPackage(package, unitOfWork);
                unitOfWork.Commit();
            }

            if (success)
            {
                return HttpResponse.GetSuccessResponse();
            }
            else
            {
                return HttpResponse.GetInternalServerErrorResponse();
            }
        }

        [Post]
        public HttpResponse BuyPackage(SessionContext context)
        {
            if (!context.UserId.HasValue)
            {
                return HttpResponse.GetUnauthorizedResponse();
            }

            User user = userRepository.GetUserById(context.UserId.Value, queryDatabase);

            Package package = packageRepository.GetRandomActivePackage(queryDatabase);
            if (package == null)
            {
                return new HttpResponse(404, "No Card package available for buying");
            }

            if (user.Coins < package.Price)
            {
                return new HttpResponse(403, "User doesn´t have enough coins.");
            }

            user.Cards.AddRange(package.CardIds);
            user.Coins -= package.Price;
            package.Active = false;

            using (IUnitOfWork unitOfWork = unitOfWorkFactory.CreateAndBeginTransaction())
            {
                bool success = packageRepository.UpdatedPackage(package, unitOfWork);
                bool success2 = userRepository.UpdateUser(user, unitOfWork);

                unitOfWork.Commit();
            }

            return HttpResponse.GetSuccessResponse();
        }

        private readonly IPackageRepository packageRepository;
        private readonly IUserRepository userRepository;
        private readonly IQueryDatabase queryDatabase;
        private readonly PackageFactory packageFactory;
        private readonly UnitOfWorkFactory unitOfWorkFactory;
        private readonly Random rnd;
    }
}
