
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

        public StatsModule( IUserStatisticRepository userStatisticRepository, IQueryDatabase queryDatabase)
        {
            this.userStatisticRepository = userStatisticRepository;
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
            List<ScoreboardRow> scoreboard = userStatisticRepository.GetAllUserStats(queryDatabase)
                .Select(stat => new ScoreboardRow(stat.Username, stat.Wins))
                .OrderByDescending(scoreRow => scoreRow.Wins)
                .ToList();

            HttpResponse httpResponse= HttpResponse.GetSuccessResponse();
            httpResponse.Content = JsonConvert.SerializeObject(scoreboard);
            return httpResponse;
        }

        private readonly IUserStatisticRepository userStatisticRepository;
        private readonly IQueryDatabase queryDatabase;
    }
}
