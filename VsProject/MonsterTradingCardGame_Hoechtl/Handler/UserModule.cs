using MonsterTradingCardGame_Hoechtl.Handler.HttpAttributes;
using MonsterTradingCardGame_Hoechtl.Infrastructure;
using MonsterTradingCardGame_Hoechtl.Models;
using MTCG.Logic.Infrastructure.Repositories;
using MTCG.Models;
using Newtonsoft.Json;

namespace MonsterTradingCardGame_Hoechtl.Handler
{
    internal class UserModule : IHandler
    {
        public string ModuleName => "Users";

        public UserModule(IUserRepository userRepository, SessionService sessionService)
        {
            this.userRepository = userRepository;
            this.sessionService = sessionService;
        }

        [Post]
        public HttpResponse RegisterUser(SessionContext context, UserCredentials userCredentials)
        {
            bool alreadyInDatabase = userRepository.GetUserByUsername(userCredentials.UserName) != null;
            if (alreadyInDatabase)
            {
                return new HttpResponse(409, "User with same username already registered", string.Empty);
            }

            // encrpyt passwort hier
            bool success = userRepository.SaveUser(userCredentials.UserName, userCredentials.Password);
            if (!success)
            {
                return new HttpResponse(400, $"Fehler beim Speichern des Users {userCredentials.UserName}.", string.Empty);
            }
            return new HttpResponse(201, "Ok", string.Empty);
        }

        [Post]
        public HttpResponse LoginUser(SessionContext context, UserCredentials userCredentials)
        {
            User user = userRepository.GetUserByUsername(userCredentials.UserName);
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
            User user = userRepository.GetUserByUsername(username);
            if (user == null)
            {
                return new HttpResponse(404, "User not found.");
            }

            if (sessionService.IsValidUsersOrAdminKey(context.SessionKey.Id, user.Id))
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
                    // update user data here
                    bool success = userRepository.UpdateUser(user);
                    if (success)
                    {
                        User updatedUser = userRepository.GetUserById(user.Id);
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

        private readonly IUserRepository userRepository;
        private readonly SessionService sessionService;
    }
}
