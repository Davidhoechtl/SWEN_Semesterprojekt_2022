using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Logic.Models.Trading
{
    internal class ElementTypRequirement : TradeRequirement
    {
        public ElementTyp ElementTyp { get; init; }
        public ElementTypRequirement(ElementTyp elementTyp)
        {
            this.ElementTyp = elementTyp;
        }

        protected override bool MeetsRequirement(Card card)
        {
            return card.ElementTyp == ElementTyp;
        }
    }
}
