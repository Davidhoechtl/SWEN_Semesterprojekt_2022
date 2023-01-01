
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

        public PackageModule(IPackageRepository packageRepository, IUserRepository userRepository, PackageFactory packageFactory, Random rnd)
        {
            this.packageRepository = packageRepository;
            this.userRepository = userRepository;
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
        public HttpResponse BuyPackage(SessionContext context)
        {
            if (!context.UserId.HasValue)
            {
                return HttpResponse.GetUnauthorizedResponse();
            }

            User user = userRepository.GetUserById(context.UserId.Value);

            Package package = packageRepository.GetRandomActivePackage();
            if (package == null)
            {
                return new HttpResponse(404, "No Card package available for buying");
            }

            if(user.Coins < package.Price)
            {
                return new HttpResponse(403, "User doesn´t have enough coins.");
            }

            user.Stack.AddRange(package.CardIds);
            user.Coins -= package.Price;
            package.Active = false;

            bool success = packageRepository.UpdatedPackage(package);
            bool success2 = userRepository.UpdateUser(user);

            if (success && success2)
            {
                return HttpResponse.GetSuccessResponse();
            }
            else
            {
                return HttpResponse.GetInternalServerErrorResponse();
            }
        }

        private readonly IPackageRepository packageRepository;
        private readonly IUserRepository userRepository;
        private readonly PackageFactory packageFactory;
        private readonly Random rnd;
    }
}
