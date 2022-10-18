using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Logic.Models.Trading
{
    public class TradingOffer
    {
        public string Id { get; private set; }
        public Card Card { get; private set; }
        public List<TradeRequirement> TradeRequirements { get; private set; } = new();
        public string SellerUserName { get; private set; }
        public string BuyerUserName { get; set; }
        public bool Active { get; private set; }

        public TradingOffer(string sellerUserName, Card card, List<TradeRequirement> tradeRequirements, bool active)
            : this(null, sellerUserName, card, tradeRequirements, active)
        {
        }

        public TradingOffer(string id, string sellerUserName, Card card, List<TradeRequirement> tradeRequirements, bool active)
        {
            this.Id = id;
            this.SellerUserName = sellerUserName;
            this.Card = card;
            this.TradeRequirements = tradeRequirements;
            this.Active = active;
        }
    }
}
