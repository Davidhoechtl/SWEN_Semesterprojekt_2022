
namespace MTCG.Logic.Models
{
    using MTCG.Models;

    public class BattleParticipant
    {
        public User User { get; set; }
        public BattleProtocol BattleProtocol { get; set; }

        public BattleParticipant(User user) 
        {
            this.User = user;
        }
    }
}
