using MTCG.Logic.Models.Trading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Logic.Infrastructure.Repositories
{
    public interface ITradeOfferRepository
    {
        bool InsertTradeOffer(TradingOffer tradeOffer);
        bool UpdateTradeOffer(TradingOffer tradeOffer);
        TradingOffer GetTradeOfferBySellerId(int sellerId);
        IEnumerable<TradingOffer> GetAllTradeOffers();
    }
}
