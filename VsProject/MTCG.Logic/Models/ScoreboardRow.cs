
namespace MTCG.Logic.Models
{
    public class ScoreboardRow
    {
        public string Username { get; set; }
        public int Elo { get; set; }

        public ScoreboardRow(string username, int elo)
        {
            Username = username;
            Elo = elo;
        }
    }
}
