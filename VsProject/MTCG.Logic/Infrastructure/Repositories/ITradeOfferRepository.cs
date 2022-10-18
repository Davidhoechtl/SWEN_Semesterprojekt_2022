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
        void SaveTradeOffer(TradingOffer tradeOffer);
        TradingOffer GetTradeOfferBySeller(string sellerUserName);
        IEnumerable<TradingOffer> GetAllTradeOffers();
    }
}
