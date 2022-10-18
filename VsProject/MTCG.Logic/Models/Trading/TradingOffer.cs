using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Logic.Models.Trading
{
    internal class TradingOffer
    {
        public Card Card { get; private set; }
        public List<TradeRequirement> TradeRequirements { get; private set; } = new();

        public TradingOffer(Card card, List<TradeRequirement> tradeRequirements)
        {
            this.Card = card;
            this.TradeRequirements = tradeRequirements;
        }
    }
}
