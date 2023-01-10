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
        public TradeLauncher(IUserRepository userRepository, ITradeOfferRepository tradeOfferRepository, IQueryDatabase queryDatabase, UnitOfWorkFactory unitOfWorkFactory)
        {
            this.userRepository = userRepository;
            this.tradeOfferRepository = tradeOfferRepository;
            this.queryDatabase = queryDatabase;
            this.unitOfWorkFactory = unitOfWorkFactory;
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

        public bool TryBuyTradeOffer(User buyer, Card providedCard, int tradeOfferId)
        {
            TradingOffer offer = tradeOfferRepository.GetTradingOfferById(tradeOfferId, queryDatabase);

            foreach(TradeRequirement requirement in offer.TradeRequirements)
            {
                if (!requirement.MeetsRequirement(providedCard))
                {
                    return false;
                }
            }

            // provided card matches all trade requirements proceed with trading cards
            User seller = userRepository.GetUserById(offer.SellerId, queryDatabase);

            // give buyer card
            seller.Cards.Remove(offer.Card);
            buyer.Cards.Add(offer.Card);

            // give seller card
            buyer.Cards.Remove(providedCard);
            seller.Cards.Add(providedCard);

            offer.BuyerId = buyer.Id;
            offer.Active = false;

            // trade completed, update database...
            using(IUnitOfWork unitOfWork = unitOfWorkFactory.CreateAndBeginTransaction())
            {
                bool success = true;
                success = userRepository.UpdateUser(seller, unitOfWork) && success;
                success = userRepository.UpdateUser(buyer, unitOfWork) && success;
                success = tradeOfferRepository.UpdateTradeOffer(offer, unitOfWork) && success;

                if (success)
                {
                    unitOfWork.Commit();
                    return true;
                }
            }

            return false;
        }

        private readonly IUserRepository userRepository;
        private readonly ITradeOfferRepository tradeOfferRepository;
        private readonly IQueryDatabase queryDatabase;
        private readonly UnitOfWorkFactory unitOfWorkFactory;
    }
}
