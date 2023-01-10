
namespace MTCG.Logic.Models.Trading
{
    using MTCG.Models;

    public class DamageRequirement : TradeRequirement
    {
        public double MinimumDamage { get; init; }
        public DamageRequirement(double minimumDamage)
        {
            this.MinimumDamage = minimumDamage;
        }
        public override bool MeetsRequirement(Card card)
        {
            return card.Damage >= MinimumDamage;
        }
    }
}
