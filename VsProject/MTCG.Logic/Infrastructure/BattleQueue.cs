using MTCG.DAL;
using MTCG.Logic.Infrastructure.Repositories;
using MTCG.Logic.Models;
using MTCG.Models;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace MTCG.Logic.Infrastructure
{
    public class BattleQueue
    {
        public ConcurrentQueue<BattleParticipant> queuedUser { get; private set; }
        public Thread LaunchLoopThread { get; private set; }
        public BattleQueue(BattleLauncher battleLauncher, IUserRepository userRepository, UnitOfWorkFactory unitOfWorkFactory)
        {
            this.battleLauncher = battleLauncher;
            this.userRepository = userRepository;
            this.unitOfWorkFactory = unitOfWorkFactory;
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
                        Deck user1Deck = user1.User.Deck;
                        Deck user2Deck = user2.User.Deck;
                        
                        // game is played with cloned decks so the original state does not get manipulated
                        user1.User.Deck = (Deck)user1.User.Deck.Clone();
                        user2.User.Deck = (Deck)user2.User.Deck.Clone();

                        (BattleResult state, string protocol) result = battleLauncher.Launch(user1.User, user2.User);

                        if(result.state == BattleResult.Won)
                        {
                            // player 2 louses his cards
                            user2.User.Deck.Cards.Clear();
                            user2.User.Cards.RemoveRange(user2Deck.Cards);

                            // player 1 gains cards
                            user1.User.Deck = user1Deck;
                            user1.User.Cards.AddRange(user2Deck.Cards);
                            
                            UpdateDatabase(user1.User, user2.User);
                            
                            user1.BattleProtocol = new BattleProtocol(result.state, result.protocol);
                            user2.BattleProtocol = new BattleProtocol(BattleResult.Lose, result.protocol);
                        }
                        else if(result.state == BattleResult.Lose)
                        {
                            // player 1 louses his cards
                            user1.User.Deck.Cards.Clear();
                            user1.User.Cards.RemoveRange(user1Deck.Cards);

                            // player 2 gains cards
                            user2.User.Deck = user2Deck;
                            user2.User.Cards.AddRange(user1Deck.Cards);
                            
                            UpdateDatabase(user1.User, user2.User);

                            user1.BattleProtocol = new BattleProtocol(result.state, result.protocol);
                            user2.BattleProtocol = new BattleProtocol(BattleResult.Won, result.protocol);
                        }
                        else
                        {
                            // draw or invalid
                            user1.BattleProtocol = new BattleProtocol(result.state, result.protocol);
                            user2.BattleProtocol = new BattleProtocol(result.state, result.protocol);
                        }
                    }
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }

        private bool UpdateDatabase(User player1, User player2)
        {
            using IUnitOfWork unitOfWork = unitOfWorkFactory.CreateAndBeginTransaction();
            bool successUser1 = userRepository.UpdateUser(player1, unitOfWork);
            bool successUser2 = userRepository.UpdateUser(player2, unitOfWork);

            if(successUser1 && successUser2)
            {
                unitOfWork.Commit();
                return true;
            }

            return false;
        }

        private readonly BattleLauncher battleLauncher;
        private readonly IUserRepository userRepository;
        private readonly UnitOfWorkFactory unitOfWorkFactory;
    }
}
