using MTCG.Models;

namespace MTCG.Logic.Models.Trading
{
    public class TradingOffer
    {
        public int Id { get; private set; }
        public Card Card { get; private set; }
        public List<TradeRequirement> TradeRequirements { get; private set; } = new();
        public int SellerId { get; private set; }
        public int? BuyerId { get; set; }
        public bool Active { get; private set; }

        public TradingOffer(int sellerId, Card card, List<TradeRequirement> tradeRequirements, bool active)
            : this(0, sellerId, null, card, tradeRequirements, active)
        {
        }

        public TradingOffer(int id, int sellerId, int? buyerId, Card card, List<TradeRequirement> tradeRequirements, bool active)
        {
            this.Id = id;
            this.SellerId = sellerId;
            this.BuyerId = buyerId;
            this.Card = card;
            this.TradeRequirements = tradeRequirements;
            this.Active = active;
        }

        public T GetTradRequirement<T>() 
            where T : TradeRequirement
        {
            foreach(TradeRequirement requirement in this.TradeRequirements)
            {
                if( requirement is T)
                {
                    return (T)requirement;
                }
            }

            return null;
        }
    }
}
