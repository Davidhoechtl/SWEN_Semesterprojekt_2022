
namespace MTCG.Logic.Models
{
    public class UserStatistic
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public int CoinsSpent { get; set; }
        public int BattlesPlayed { get; set; }
        public int Wins { get; set; }
        public double WinRate => Wins / (BattlesPlayed == 0 ? 1 : BattlesPlayed);
    }
}
