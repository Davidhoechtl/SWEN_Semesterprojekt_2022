using MTCG.DAL;
using MTCG.Logic.Models.Trading;

namespace MTCG.Logic.Infrastructure.Repositories
{
    public interface ITradeOfferRepository
    {
        bool InsertTradeOffer(TradingOffer tradeOffer, IQueryDatabase queryDatabase);
        bool UpdateTradeOffer(TradingOffer tradeOffer, IQueryDatabase queryDatabase);
        List<TradingOffer> GetTradeOffersBySellerId(int sellerId, IQueryDatabase queryDatabase);
        IEnumerable<TradingOffer> GetAllTradeOffers(IQueryDatabase queryDatabase);
        TradingOffer GetTradingOfferById(int tradeId, IQueryDatabase queryDatabase);
    }
}
