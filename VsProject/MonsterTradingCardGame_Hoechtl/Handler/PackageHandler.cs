
using MonsterTradingCardGame_Hoechtl.Handler.HttpAttributes;
using MonsterTradingCardGame_Hoechtl.Models;
using MTCG.Infrastructure;

namespace MonsterTradingCardGame_Hoechtl.Handler
{
    internal class PackageHandler : IHandler
    {
        public string ModuleName => "Package";

        public PackageHandler( PackageFactory packageFactory)
        {
            this.packageFactory = packageFactory;
        }

        [Post]
        public HttpResponse CreateNewPackage(SessionContext context, int minItems, int maxItems)
        {
            return HttpResponse.GetSuccessResponse();
        }

        [Post]
        public HttpResponse BuyPackage( SessionContext context, int userId, int packageId)
        {
            return HttpResponse.GetSuccessResponse();
        }

        private readonly PackageFactory packageFactory;
    }
}
