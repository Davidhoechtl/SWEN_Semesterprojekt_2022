using MTCG.Logic.Models;
using MTCG.Models;
using System.Collections.Concurrent;

namespace MTCG.Logic.Infrastructure
{
    public class BattleQueue
    {
        public ConcurrentQueue<BattleParticipant> queuedUser { get; private set; }
        public Thread LaunchLoopThread { get; private set; }
        public BattleQueue(BattleLauncher battleLauncher)
        {
            this.battleLauncher = battleLauncher;
            this.queuedUser = new ConcurrentQueue<BattleParticipant>();

            LaunchLoopThread = new Thread(LaunchLoop);
            LaunchLoopThread.Start();
        }

        public BattleProtocol QueueForBattle(User user)
        {
            BattleParticipant battleParticipant = new BattleParticipant(user);
            queuedUser.Enqueue(battleParticipant);

            while (battleParticipant.BattleProtocol == null)
            {
                Thread.Sleep(1000);
            }

            return battleParticipant.BattleProtocol;
        }

        public void LaunchLoop()
        {
            while (true)
            {
                if (queuedUser.Count > 1)
                {
                    if (queuedUser.TryDequeue(out BattleParticipant user1) &&
                        queuedUser.TryDequeue(out BattleParticipant user2))
                    {
                        BattleResult result = battleLauncher.Launch(user1.User, user2.User);

                        if(result == BattleResult.Won)
                        {
                            user1.BattleProtocol = new BattleProtocol(result, string.Empty);
                            user2.BattleProtocol = new BattleProtocol(BattleResult.Lose, string.Empty);

                        }
                        else if(result == BattleResult.Lose)
                        {
                            user1.BattleProtocol = new BattleProtocol(result, string.Empty);
                            user2.BattleProtocol = new BattleProtocol(BattleResult.Won, string.Empty);
                        }
                        else
                        {
                            // draw or invalid
                            user1.BattleProtocol = new BattleProtocol(result, string.Empty);
                            user2.BattleProtocol = new BattleProtocol(result, string.Empty);
                        }
                    }
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }

        private readonly BattleLauncher battleLauncher;
    }
}
