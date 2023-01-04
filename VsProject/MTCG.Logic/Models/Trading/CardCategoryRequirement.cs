
namespace MTCG.Logic.Models.Trading
{
    using MTCG.Models;
    
    public class CardCategoryRequirement : TradeRequirement
    {
        public Type CardType { get; private set; }
        public CardCategoryRequirement(Type type)
        {
            CardType = type;
        }

        protected override bool MeetsRequirement(Card card)
        {
            return card.GetType() == CardType;
        }
    }
}
