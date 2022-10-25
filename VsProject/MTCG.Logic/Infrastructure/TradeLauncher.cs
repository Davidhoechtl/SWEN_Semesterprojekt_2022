using MTCG.Logic.Infrastructure.Repositories;
using MTCG.Logic.Models.Trading;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Logic.Infrastructure
{
    public class TradeLauncher
    {
        public TradeLauncher(ITradeOfferRepository tradeOfferRepository)
        {
            this.tradeOfferRepository = tradeOfferRepository;
        }

        public void SaveTradeOffer(User user, Card card, params TradeRequirement[] requirements)
        {
            TradingOffer offer = new TradingOffer(
                user.Credentials.UserName, 
                card, 
                requirements.ToList(), 
                true
            );

            tradeOfferRepository.SaveTradeOffer(offer);
        }

        private readonly ITradeOfferRepository tradeOfferRepository;
    }
}
