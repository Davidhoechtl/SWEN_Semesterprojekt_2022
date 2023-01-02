

namespace MonsterTradingCardGame_Hoechtl.Handler
{
    using MonsterTradingCardGame_Hoechtl.Handler.HttpAttributes;
    using MonsterTradingCardGame_Hoechtl.Models;
    using MTCG.DAL;
    using MTCG.Logic.Infrastructure.Repositories;
    using MTCG.Models;
    using Newtonsoft.Json;
    using System.Linq;

    internal class DeckModule : IHandler
    {
        public const int MinDeckAmount = 4;
        public string ModuleName => "Deck";

        public DeckModule(
            IQueryDatabase queryDatabase,
            UnitOfWorkFactory unitOfWorkFactory,
            IUserRepository userRepository,
            IDeckRepository deckRepository )
        {
            this.queryDatabase = queryDatabase;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.userRepository = userRepository;
            this.deckRepository = deckRepository;
        }

        [Get]
        public HttpResponse GetUserDeck(SessionContext context)
        {
            if (!context.UserId.HasValue)
            {
                return HttpResponse.GetUnauthorizedResponse();
            }

            User user = userRepository.GetUserById(context.UserId.Value, queryDatabase);
            string deckAsJson = JsonConvert.SerializeObject(user.Deck);

            HttpResponse response = HttpResponse.GetSuccessResponse();
            response.Content = deckAsJson;
            return response;
        }

        [Post]
        public HttpResponse ConfigureUserDeck(SessionContext context, int[] deckCards)
        {
            if (!context.UserId.HasValue)
            {
                return HttpResponse.GetUnauthorizedResponse();
            }

            if(deckCards.Count() < MinDeckAmount)
            {
                return new HttpResponse(400, "The provided deck did not include the required amount of cards.");
            }

            User user = userRepository.GetUserById(context.UserId.Value, queryDatabase);
            user.Deck.Cards.Clear();

            foreach(int cardId in deckCards)
            {
                Card card = user.Cards.FirstOrDefault(c => c.Id == cardId);
                if(card == null)
                {
                    return new HttpResponse(403, "At least one of the provided cards does not belong to the user or is not available.");
                }

                user.Deck.Cards.Push(card);
            }

            using(IUnitOfWork unitOfWork = unitOfWorkFactory.CreateAndBeginTransaction())
            {
                deckRepository.UpdateUsersDeck(user.Deck, unitOfWork);
                unitOfWork.Commit();
            }

            return HttpResponse.GetSuccessResponse();
        }

        private readonly IQueryDatabase queryDatabase;
        private readonly UnitOfWorkFactory unitOfWorkFactory;
        private readonly IUserRepository userRepository;
        private readonly IDeckRepository deckRepository;
    }
}
