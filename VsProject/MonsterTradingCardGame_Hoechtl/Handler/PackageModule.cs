
namespace MonsterTradingCardGame_Hoechtl.Handler
{
    using MonsterTradingCardGame_Hoechtl.Handler.HttpAttributes;
    using MonsterTradingCardGame_Hoechtl.Models;
    using MTCG.Infrastructure;
    using MTCG.Logic.Infrastructure.Repositories;
    using MTCG.Logic.Models.RequestContexts;
    using MTCG.Models;

    internal class PackageModule : IHandler
    {
        public string ModuleName => "Package";

        public PackageModule(IPackageRepository packageRepository, PackageFactory packageFactory, Random rnd)
        {
            this.packageRepository = packageRepository;
            this.packageFactory = packageFactory;
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

            bool success = packageRepository.InsertPackage(package);

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
        public HttpResponse BuyPackage(SessionContext context, int packageId)
        {
            if (context.UserId.HasValue)
            {
                return HttpResponse.GetUnauthorizedResponse();
            }

            int userId = context.UserId.Value;

            Package package = packageRepository.GetRandomActivePackage();
            if(package == null)
            {
                return new HttpResponse(404, "No Card package available for buying");
            }

            return HttpResponse.GetSuccessResponse();
        }

        private readonly IPackageRepository packageRepository;
        private readonly PackageFactory packageFactory;
        private readonly Random rnd;
    }
}
