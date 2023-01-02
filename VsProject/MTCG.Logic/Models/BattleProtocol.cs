
namespace MTCG.Logic.Models
{
    public class BattleProtocol
    {
        public BattleResult BattleResult { get; set; }
        public string BattleResultAsString => BattleResult.ToString();
        public string Protocol { get; set; }

        public BattleProtocol(BattleResult battleResult, string protocol)
        {
            BattleResult = battleResult;
            Protocol = protocol;
        }
    }
}
