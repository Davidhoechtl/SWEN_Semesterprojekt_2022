using MonsterTradingCardGame_Hoechtl.Handler.HttpAttributes;
using MonsterTradingCardGame_Hoechtl.Infrastructure;
using MonsterTradingCardGame_Hoechtl.Models;
using MTCG.DAL;
using MTCG.Logic.Infrastructure.Repositories;
using MTCG.Models;
using Newtonsoft.Json;

namespace MonsterTradingCardGame_Hoechtl.Handler
{
    internal class UserModule : IHandler
    {
        public const int StartCoins = 100;
        public string ModuleName => "Users";

        public UserModule(
            IQueryDatabase queryDatabase,
            IUserRepository userRepository,
            UnitOfWorkFactory unitOfWorkFactory,
            SessionService sessionService)
        {
            this.queryDatabase = queryDatabase;
            this.userRepository = userRepository;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.sessionService = sessionService;
        }

        [Post]
        public HttpResponse RegisterUser(SessionContext context, UserCredentials userCredentials)
        {
            bool alreadyInDatabase = userRepository.GetUserByUsername(userCredentials.UserName, queryDatabase) != null;
            if (alreadyInDatabase)
            {
                return new HttpResponse(409, "User with same username already registered", string.Empty);
            }

            // encrpyt passwort hier
            bool success;
            using (IUnitOfWork unitOfWork = unitOfWorkFactory.CreateAndBeginTransaction())
            {
                success = userRepository.RegisterUser(userCredentials.UserName, userCredentials.Password, StartCoins, unitOfWork);
                if (success)
                {
                    unitOfWork.Commit();
                    return HttpResponse.GetSuccessResponse();
                }
                else
                {
                    return new HttpResponse(400, $"Fehler beim Speichern des Users {userCredentials.UserName}.", string.Empty);
                }
            }
        }

        [Post]
        public HttpResponse LoginUser(SessionContext context, UserCredentials userCredentials)
        {
            User user = userRepository.GetUserByUsername(userCredentials.UserName, queryDatabase);
            if (user != null)
            {
                // decrypt password
                if (user.Credentials.Password.Equals(userCredentials.Password, StringComparison.Ordinal))
                {
                    // retrun session Token
                    return new HttpResponse(200, "User login successful", sessionService.CreateNewSessionKey(user.Id).ToString());
                }
            }

            return new HttpResponse(401, "Invalid username/password provided", string.Empty);
        }

        [Get]
        public HttpResponse GetUser(SessionContext context, string username)
        {
            User user = userRepository.GetUserByUsername(username, queryDatabase);
            if (user == null)
            {
                return new HttpResponse(404, "User not found.");
            }

            if (sessionService.IsValidUsersOrAdminKey(context.SessionKey?.Id, user.Id))
            {
                string userData = JsonConvert.SerializeObject(user);
                return new HttpResponse(200, "OK")
                {
                    Content = userData
                };
            }
            else
            {
                return HttpResponse.GetUnauthorizedResponse();
            }
        }

        [Put]
        public HttpResponse UpdateUser(SessionContext context, User user)
        {
            if (user != null)
            {
                if (sessionService.IsValidUsersOrAdminKey(context.SessionKey.Id, user.Id))
                {
                    bool success = false;
                    using (IUnitOfWork unitOfWork = unitOfWorkFactory.CreateAndBeginTransaction())
                    {
                        // update user data here
                        success = userRepository.UpdateUser(user, unitOfWork);
                        unitOfWork.Commit();
                    }

                    if (success)
                    {
                        User updatedUser = userRepository.GetUserById(user.Id, queryDatabase);
                        string userData = JsonConvert.SerializeObject(updatedUser);

                        return new HttpResponse(200, "User sucessfully updated.")
                        {
                            Content = userData
                        };
                    }
                    else
                    {
                        return new HttpResponse(500, "User was found, but there was an intern error");
                    }
                }
                else
                {
                    return HttpResponse.GetUnauthorizedResponse();
                }
            }

            return new HttpResponse(404, "User not found.");
        }

        private readonly IQueryDatabase queryDatabase;
        private readonly IUserRepository userRepository;
        private readonly UnitOfWorkFactory unitOfWorkFactory;
        private readonly SessionService sessionService;
    }
}
