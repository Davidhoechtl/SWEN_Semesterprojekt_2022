using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Logic.Models.Trading
{
    public class DamageRequirement : TradeRequirement
    {
        public int MinimumDamage { get; init; }
        public DamageRequirement(int minimumDamage)
        {
            this.MinimumDamage = minimumDamage;
        }
        protected override bool MeetsRequirement(Card card)
        {
            return card.Damage >= MinimumDamage;
        }
    }
}
