using MTCG.Logic.Models.Trading;
using MTCG.Models;

namespace MTCG.Logic.Infrastructure.Repositories.MockUps
{
    internal class MockTradeOfferRepository : ITradeOfferRepository
    {
        private static readonly IEnumerable<TradingOffer> offers = new List<TradingOffer>()
        {
            new TradingOffer(
                "Offer1",
                "TestUser",
                new MonsterCard()
                {
                    Name = "Wolve",
                    Damage = 10,
                    ElementTyp = ElementTyp.Normal
                },
                new List<TradeRequirement>()
                {
                    new DamageRequirement(15),
                    new ElementTypRequirement(ElementTyp.Fire),
                    new CardTypRequirement<MonsterCard>()
                },
                true
            ),
            new TradingOffer(
                "Offer2",
                "TestUser2",
                new MonsterCard()
                {
                    Name = "Blob",
                    Damage = 10,
                    ElementTyp = ElementTyp.Normal
                },
                new List<TradeRequirement>()
                {
                    new DamageRequirement(15),
                    new ElementTypRequirement(ElementTyp.Fire),
                    new CardTypRequirement<SpellCard>()
                },
                false
            ) { BuyerUserName = "TestUser" },
        };

        public IEnumerable<TradingOffer> GetAllTradeOffers()
        {
            return offers;
        }

        public TradingOffer GetTradeOfferBySeller(string sellerUserName)
        {
            return offers
                .FirstOrDefault(o => o.SellerUserName.Equals(sellerUserName, StringComparison.Ordinal)
            );
        }

        public void SaveTradeOffer(TradingOffer tradeOffer)
        {
            throw new NotImplementedException();
        }
    }
}
