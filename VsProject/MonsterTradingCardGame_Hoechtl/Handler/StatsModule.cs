
namespace MonsterTradingCardGame_Hoechtl.Handler
{
    using MonsterTradingCardGame_Hoechtl.Handler.HttpAttributes;
    using MonsterTradingCardGame_Hoechtl.Models;
    using MTCG.DAL;
    using MTCG.Logic.Infrastructure.Repositories;
    using MTCG.Logic.Models;
    using Newtonsoft.Json;

    internal class StatsModule : IHandler
    {
        public string ModuleName => "Stats";

        public StatsModule( IUserStatisticRepository userStatisticRepository, IUserRepository userRepository, IQueryDatabase queryDatabase)
        {
            this.userStatisticRepository = userStatisticRepository;
            this.userRepository = userRepository;
            this.queryDatabase = queryDatabase;
        }

        [Get]
        public HttpResponse GetUserStats(SessionContext context)
        {
            if (!context.UserId.HasValue)
            {
                return HttpResponse.GetUnauthorizedResponse();
            }

            UserStatistic userStatistic = userStatisticRepository.GetStatisticByUserId(context.UserId.Value, queryDatabase);
            if(userStatistic != null)
            {
                HttpResponse response = HttpResponse.GetSuccessResponse();
                response.Content = JsonConvert.SerializeObject(userStatistic);
                return response;
            }
            else
            {
                return HttpResponse.GetInternalServerErrorResponse();
            }
        }

        [Get]
        public HttpResponse ShowScoreboard(SessionContext context)
        {
            List<ScoreboardRow> scoreboard = userRepository.GetAllUsersCore(queryDatabase)
                .Select(user => new ScoreboardRow(user.Credentials.UserName, user.ELO))
                .OrderByDescending(scoreRow => scoreRow.Elo)
                .ToList();

            HttpResponse httpResponse= HttpResponse.GetSuccessResponse();
            httpResponse.Content = JsonConvert.SerializeObject(scoreboard);
            return httpResponse;
        }

        private readonly IUserStatisticRepository userStatisticRepository;
        private readonly IUserRepository userRepository;
        private readonly IQueryDatabase queryDatabase;
    }
}
