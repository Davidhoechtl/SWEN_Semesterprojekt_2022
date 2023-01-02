
namespace MonsterTradingCardGame_Hoechtl.Handler
{
    using MonsterTradingCardGame_Hoechtl.Handler.HttpAttributes;
    using MonsterTradingCardGame_Hoechtl.Models;
    using MTCG.DAL;
    using MTCG.Logic.Infrastructure.Repositories;
    using MTCG.Models;
    using Newtonsoft.Json;

    internal class CardModule : IHandler
    {
        public string ModuleName => "Cards";

        public CardModule( ICardRepository cardRepository, IQueryDatabase queryDatabase)
        {
            this.cardRepository = cardRepository;
            this.queryDatabase = queryDatabase;
        }

        [Get]
        public HttpResponse GetUserCards(SessionContext context)
        {
            if (!context.UserId.HasValue)
            {
                return HttpResponse.GetUnauthorizedResponse();
            }

            List<Card> usersCards = cardRepository
                .GetUserCards(context.UserId.Value, queryDatabase)
                .ToList();

            if(usersCards.Count < 0)
            {
                return new HttpResponse(204, "The request was fine, but the user doesn't have any cards");
            }

            string cardsAsJson = JsonConvert.SerializeObject(usersCards);
            HttpResponse successResponse = HttpResponse.GetSuccessResponse();
            successResponse.Content = cardsAsJson;
            return successResponse;
        }

        private readonly ICardRepository cardRepository;
        private readonly IQueryDatabase queryDatabase;
    }
}
