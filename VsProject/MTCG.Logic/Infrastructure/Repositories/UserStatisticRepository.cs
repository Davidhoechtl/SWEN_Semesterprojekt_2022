
namespace MTCG.Logic.Infrastructure.Repositories
{
    using MTCG.DAL;
    using MTCG.Logic.Models;
    using Npgsql;
    using System.Collections.Generic;

    public class UserStatisticRepository : IUserStatisticRepository
    {
        public IEnumerable<UserStatistic> GetAllUserStats(IQueryDatabase queryDatabase)
        {
            string sqlStatement = 
                @"SELECT users.username, users_stats.* 
                  FROM users_stats
                  JOIN users ON (users_stats.user_id = users.user_id)";

            List<UserStatistic> stats = queryDatabase.GetItems<UserStatistic>(
                sqlStatement,
                reader =>
                {
                    List<UserStatistic> userStats = new();
                    while (reader.Read())
                    {
                        userStats.Add(GetUserStatisticFromDataReader(reader));
                    }
                    reader.Close();
                    return userStats;
                }
            );

            return stats;
        }

        public UserStatistic GetStatisticByUserId(int userId, IQueryDatabase queryDatabase)
        {
            string sqlStatement =
                @"SELECT users.username, users_stats.* 
                  FROM users_stats
                  JOIN users ON (users_stats.user_id = users.user_id)
                  WHERE users_stats.user_id = @userId";

            UserStatistic stats = queryDatabase.GetItem(
                sqlStatement,
                reader => 
                {
                    UserStatistic statistic = GetUserStatisticFromDataReader(reader);
                    reader.Close();
                    return statistic;
                },
                new NpgsqlParameter("userId", userId)
            );

            return stats;
        }

        public bool UpdateUserStatistic(UserStatistic userStatistic, IUnitOfWork unitOfWork)
        {
            string sqlStatement = "UPDATE users_stats SET username = @username, coins_spent = @coinsSpent, battles_played = @battlesPlayed, wins = @wins, win_rate = @winRate WHERE stats_id = @statsId";

            int affectedRows = unitOfWork.ExecuteNonQuery(
                sqlStatement,
                new NpgsqlParameter("statsId", userStatistic.Id),
                new NpgsqlParameter("username", userStatistic.Username),
                new NpgsqlParameter("coinsSpent", userStatistic.CoinsSpent),
                new NpgsqlParameter("battlesPlayed", userStatistic.BattlesPlayed),
                new NpgsqlParameter("wins", userStatistic.Wins),
                new NpgsqlParameter("win", userStatistic.CoinsSpent),
                new NpgsqlParameter("winRate", userStatistic.WinRate)
            );

            return affectedRows != 0;
        }

        private UserStatistic GetUserStatisticFromDataReader(NpgsqlDataReader reader)
        {
            UserStatistic statistic = null;

            if (reader.IsOnRow)
            {
                statistic = new UserStatistic()
                {
                    Id = reader.GetInt32(reader.GetOrdinal("stats_id")),
                    UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                    Username = reader.GetString(reader.GetOrdinal("username")),
                    CoinsSpent = reader.GetInt32(reader.GetOrdinal("coins_spent")),
                    BattlesPlayed = reader.GetInt32(reader.GetOrdinal("battles_played")),
                    Wins = reader.GetInt32(reader.GetOrdinal("wins"))
                };
            }

            return statistic;
        }
    }
}
