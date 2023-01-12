using MTCG.DAL;
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
        public TradeLauncher(ITradeOfferRepository tradeOfferRepository, IQueryDatabase queryDatabase)
        {
            this.tradeOfferRepository = tradeOfferRepository;
            this.queryDatabase = queryDatabase;
        }

        public bool SaveTradeOffer(User user, Card card, params TradeRequirement[] requirements)
        {
            TradingOffer offer = new TradingOffer(
                user.Id,
                card,
                requirements.ToList(),
                true
            );

            return tradeOfferRepository.InsertTradeOffer(offer, queryDatabase);
        }

        public bool TryBuyTradeOffer(TradingOffer offer, User seller, User buyer, Card providedCard)
        {
            if(offer == null || !offer.Active)
            {
                return false;
            }

            foreach (TradeRequirement requirement in offer.TradeRequirements)
            {
                if (!requirement.MeetsRequirement(providedCard))
                {
                    return false;
                }
            }

            // provided card matches all trade requirements proceed with trading cards
            // give buyer card
            seller.Cards.Remove(offer.Card);
            buyer.Cards.Add(offer.Card);

            // give seller card
            buyer.Cards.Remove(providedCard);
            seller.Cards.Add(providedCard);

            offer.BuyerId = buyer.Id;
            offer.Active = false;

            return true;
        }

        private readonly ITradeOfferRepository tradeOfferRepository;
        private readonly IQueryDatabase queryDatabase;
    }
}
