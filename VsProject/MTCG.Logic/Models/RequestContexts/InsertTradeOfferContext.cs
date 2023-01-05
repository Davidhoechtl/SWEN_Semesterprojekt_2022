
namespace MTCG.Logic.Models.RequestContexts
{
    public class InsertTradeOfferContext
    {
        public int CardId { get; set; }
        public char? CardTypeRequirement { get; set; }
        public char? ElementRequirement { get; set; }
        public double? DamageRequirement { get; set; }
        public string CategoryRequirement { get; set; }
    }
}
