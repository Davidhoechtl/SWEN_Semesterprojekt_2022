
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
            CardFactory cardFactory,
            TradeLauncher tradeLauncher)
        {
            this.userRepository = userRepository;
            this.tradeOfferRepository = tradeOfferRepository;
            this.queryDatabase = queryDatabase;
            this.cardFactory = cardFactory;
            this.tradeLauncher = tradeLauncher;
        }

        [Post]
        public HttpResponse InsertOffer(SessionContext context, InsertTradeOfferContext tradeOfferContext)
        {
            if (!context.UserId.HasValue)
            {
                return HttpResponse.GetUnauthorizedResponse();
            }

            User user = userRepository.GetUserById(context.UserId.Value, queryDatabase);
            Card offeredCard = user.Cards.FirstOrDefault(card => card.Id == tradeOfferContext.CardId);
            if (offeredCard == null)
            {
                // User does not have the card he wants to offer
                return new HttpResponse(403, "The user does not have the card he wants to offer.");
            }

            List<TradeRequirement> tradeRequirements = GetTradeRequirementsFromContext(tradeOfferContext);
            TradingOffer offer = new TradingOffer(
                sellerId: user.Id,
                card: offeredCard,
                tradeRequirements,
                active: true
            );

            bool success = tradeLauncher.SaveTradeOffer(user, offeredCard, tradeRequirements.ToArray());
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
        public HttpResponse BuyTradeOffer(SessionContext context, BuyTradeOfferContext buyContext)
        {
            if (!context.UserId.HasValue)
            {
                return HttpResponse.GetUnauthorizedResponse();
            }

            User user = userRepository.GetUserById(context.UserId.Value, queryDatabase);
            Card providedCard = user.Cards.FirstOrDefault(card => card.Id == buyContext.CardId);
            if (providedCard == null)
            {
                // User does not have the card he wants to offer
                return new HttpResponse(403, "The user does not have the card he wants to provide.");
            }

            bool success = tradeLauncher.TryBuyTradeOffer(user, providedCard, buyContext.TradeId);
            if (success)
            {
                return HttpResponse.GetSuccessResponse();
            }
            else
            {
                return HttpResponse.GetInternalServerErrorResponse();
            }
        }

        private List<TradeRequirement> GetTradeRequirementsFromContext(InsertTradeOfferContext context)
        {
            List<TradeRequirement> tradeRequirements = new();

            if (context.CardTypeRequirement.HasValue)
            {
                if (context.CardTypeRequirement.ToString().Equals("M", StringComparison.OrdinalIgnoreCase))
                {
                    tradeRequirements.Add(new CardTypRequirement<MonsterCard>());
                }
                else if (context.CardTypeRequirement.ToString().Equals("S", StringComparison.OrdinalIgnoreCase))
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
                ElementTyp convertedElementType = cardFactory.ConvertCharToElementTyp(context.ElementRequirement.Value);
                tradeRequirements.Add(new ElementTypRequirement(convertedElementType));
            }
            if (!string.IsNullOrEmpty(context.CategoryRequirement))
            {
                Type convertedCardType = cardFactory.CardTypes.First(type => type.Name == context.CategoryRequirement);
                if (convertedCardType != null)
                {
                    tradeRequirements.Add(new CardCategoryRequirement(convertedCardType));
                }
            }

            return tradeRequirements;
        }

        private readonly IUserRepository userRepository;
        private readonly ITradeOfferRepository tradeOfferRepository;
        private readonly IQueryDatabase queryDatabase;
        private readonly CardFactory cardFactory;
        private readonly TradeLauncher tradeLauncher;
    }
}
