
namespace MTCG.Logic.Models
{
    public class ScoreboardRow
    {
        public string Username { get; set; }
        public int Wins { get; set; }

        public ScoreboardRow(string username, int wins)
        {
            Username = username;
            Wins = wins;
        }
    }
}
