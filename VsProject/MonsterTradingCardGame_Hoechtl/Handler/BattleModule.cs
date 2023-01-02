

namespace MonsterTradingCardGame_Hoechtl.Handler
{
    using MonsterTradingCardGame_Hoechtl.Handler.HttpAttributes;
    using MonsterTradingCardGame_Hoechtl.Models;
    using MTCG.DAL;
    using MTCG.Logic.Infrastructure;
    using MTCG.Logic.Infrastructure.Repositories;
    using MTCG.Logic.Models;
    using MTCG.Models;
    using Newtonsoft.Json;

    internal class BattleModule : IHandler
    {
        public string ModuleName => "Battle";

        public BattleModule( 
            BattleQueue battleQueue,
            IUserRepository userRepository, 
            IQueryDatabase queryDatabase, 
            UnitOfWorkFactory unitOfWorkFactory)
        {
            this.battleQueue = battleQueue;
            this.userRepository = userRepository;
            this.queryDatabase = queryDatabase;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        [Post]
        public HttpResponse Start(SessionContext context)
        {
            if(!context.UserId.HasValue)
            {
                return HttpResponse.GetUnauthorizedResponse();
            }

            User user = userRepository.GetUserById(context.UserId.Value, queryDatabase);
            BattleProtocol battleProtocol = battleQueue.QueueForBattle(user);

            string battleProtocolJson = JsonConvert.SerializeObject(battleProtocol);
            HttpResponse response = HttpResponse.GetSuccessResponse();
            response.Content = battleProtocolJson;
            return response;
        }

        private readonly BattleQueue battleQueue;
        private readonly IUserRepository userRepository;
        private readonly IQueryDatabase queryDatabase;
        private readonly UnitOfWorkFactory unitOfWorkFactory;
    }
}
