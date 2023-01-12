using MTCG.DAL;
using MTCG.Logic.Models.Trading;
using MTCG.Models;

namespace MTCG.Logic.Infrastructure.Repositories.MockUps
{
    internal class MockTradeOfferRepository : ITradeOfferRepository
    {
        public static readonly IEnumerable<TradingOffer> offers = new List<TradingOffer>()
        {
            new TradingOffer(
                1,
                1,
                null,
                new Troll()
                {
                    Name = "SmallTroll",
                    Damage = 30,
                    ElementTyp = ElementTyp.Normal
                },
                new List<TradeRequirement>()
                {
                    new DamageRequirement(60),
                    new ElementTypRequirement(ElementTyp.Water),
                    new CardTypRequirement<MonsterCard>()
                },
                true
            ),
            new TradingOffer(
                2,
                1,
                null,
                new Troll()
                {
                    Name = "SmallTroll",
                    Damage = 30,
                    ElementTyp = ElementTyp.Normal
                },
                new List<TradeRequirement>()
                {
                    new DamageRequirement(20),
                    new ElementTypRequirement(ElementTyp.Fire),
                    new CardTypRequirement<SpellCard>()
                },
                false
            ) { BuyerId = 1},
        };

        public IEnumerable<TradingOffer> GetAllTradeOffers()
        {
            return offers;
        }

        public IEnumerable<TradingOffer> GetAllTradeOffers(IQueryDatabase queryDatabase)
        {
            throw new NotImplementedException();
        }

        public TradingOffer GetTradeOfferBySeller(string sellerUserName)
        {
            //return offers
            //    .FirstOrDefault(o => o.SellerId.Equals(sellerUserName, StringComparison.Ordinal)
            //);
            throw new NotImplementedException();
        }

        public List<TradingOffer> GetTradeOffersBySellerId(int sellerId, IQueryDatabase queryDatabase)
        {
            throw new NotImplementedException();
        }

        public TradingOffer GetTradingOfferById(int tradeId, IQueryDatabase queryDatabase)
        {
            throw new NotImplementedException();
        }

        public void InsertTradeOffer(TradingOffer tradeOffer)
        {
            throw new NotImplementedException();
        }

        public bool InsertTradeOffer(TradingOffer tradeOffer, IQueryDatabase queryDatabase)
        {
            throw new NotImplementedException();
        }

        public bool UpdateTradeOffer(TradingOffer tradeOffer, IQueryDatabase queryDatabase)
        {
            throw new NotImplementedException();
        }
    }
}
