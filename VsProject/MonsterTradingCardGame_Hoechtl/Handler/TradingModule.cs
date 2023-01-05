
namespace MonsterTradingCardGame_Hoechtl.Handler
{
    using MonsterTradingCardGame_Hoechtl.Handler.HttpAttributes;
    using MonsterTradingCardGame_Hoechtl.Infrastructure;
    using MonsterTradingCardGame_Hoechtl.Models;
    using MTCG.DAL;
    using MTCG.Logic.Infrastructure;
    using MTCG.Logic.Infrastructure.Repositories;
    using MTCG.Logic.Models.RequestContexts;
    using MTCG.Logic.Models.Trading;
    using MTCG.Models;
    using System;
    using System.Reflection.PortableExecutable;

    internal class TradingModule : IHandler
    {
        public string ModuleName => "Trade";

        public TradingModule(
            IUserRepository userRepository,
            ITradeOfferRepository tradeOfferRepository,
            IQueryDatabase queryDatabase,
            UnitOfWorkFactory unitOfWorkFactory,
            CardFactory cardFactory)
        {
            this.userRepository = userRepository;
            this.tradeOfferRepository = tradeOfferRepository;
            this.queryDatabase = queryDatabase;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.cardFactory = cardFactory;
        }

        [Post]
        public HttpResponse InsertOffer(SessionContext context, InsertTradeOfferContext tradeOfferContext)
        {
            if (!context.UserId.HasValue)
            {
                return HttpResponse.GetUnauthorizedResponse();
            }

            User user = userRepository.GetUserById(context.UserId.Value, queryDatabase);
            if (!user.Cards.Any(card => card.Id == tradeOfferContext.CardId))
            {
                // User does not have the card he wants to offer
                return new HttpResponse(403, "The user does not have the card he wants to offer.");
            }


        }

        private List<TradeRequirement> GetTradeRequirementsFromContext(InsertTradeOfferContext context)
        {
            List<TradeRequirement> tradeRequirements = new();
            if (context.CardTypeRequirement.HasValue)
            {
                if (context.CardTypeRequirement == 'M')
                {
                    tradeRequirements.Add(new CardTypRequirement<MonsterCard>());
                }
                else if(context.CardTypeRequirement == 'S')
                {
                    tradeRequirements.Add(new CardTypRequirement<SpellCard>());
                }
                else
                {
                    throw new InvalidOperationException($"Card type {context.CardTypeRequirement} is unknown.");
                }
            }
            if (context.DamageRequirement.HasValue)
            {
                tradeRequirements.Add(new DamageRequirement(context.DamageRequirement.Value));
            }
            if (context.ElementRequirement.HasValue)
            {
                char elementRequirement = reader.GetChar(reader.GetOrdinal("element_requirement"));
                ElementTyp convertedElementType = cardFactory.ConvertCharToElementTyp(elementRequirement);
                tradeRequirements.Add(new ElementTypRequirement(convertedElementType));
            }
            if (!reader.IsDBNull(reader.GetOrdinal("category_requirement")))
            {
                string categoryRequirement = reader.GetString(reader.GetOrdinal("category_requirement"));
                Type convertedCardType = GetCardTypeByCategory(categoryRequirement);
                tradeRequirements.Add(new CardCategoryRequirement(convertedCardType));
            }
        }

        private readonly IUserRepository userRepository;
        private readonly ITradeOfferRepository tradeOfferRepository;
        private readonly IQueryDatabase queryDatabase;
        private readonly UnitOfWorkFactory unitOfWorkFactory;
        private readonly CardFactory cardFactory;
    }
}
