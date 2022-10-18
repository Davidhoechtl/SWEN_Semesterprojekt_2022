using MTCG.Logic.Models.Trading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Logic.Infrastructure.Repositories
{
    internal class MockTradeOfferRepository : ITradeOfferRepository
    {
        public IEnumerable<TradingOffer> GetAllTradeOffers()
        {
            throw new NotImplementedException();
        }

        public TradingOffer GetTradeOfferBySeller(string sellerUserName)
        {
            throw new NotImplementedException();
        }

        public void SaveTradeOffer(TradingOffer tradeOffer)
        {
            throw new NotImplementedException();
        }
    }
}
