using MonsterTradingCardGame_Hoechtl.Handler.HttpAttributes;
using MonsterTradingCardGame_Hoechtl.Infrastructure;
using MTCG.Logic.Infrastructure.Repositories;
using MTCG.Models;
using Newtonsoft.Json;

namespace MonsterTradingCardGame_Hoechtl.Handler
{
    internal class UserModule : IHandler
    {
        public string ModuleName => "Users";

        public UserModule(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [Post]
        public HttpResponse RegisterUser(UserCredentials userCredentials)
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
        public HttpResponse LoginUser(UserCredentials userCredentials)
        {
            User user = userRepository.GetUserByUsername(userCredentials.UserName);
            if (user != null)
            {
                // decrypt password
                if (user.Credentials.Password.Equals(userCredentials.Password, StringComparison.Ordinal))
                {
                    // retrun session Token
                    return new HttpResponse(200, "User login successful", string.Empty);
                }
            }

            return new HttpResponse(401, "Invalid username/password provided", string.Empty);
        }

        [Get]
        public HttpResponse GetUser(string username)
        {
            User user = userRepository.GetUserByUsername(username);
            if (user == null)
            {
                return new HttpResponse(404, "User not found.", string.Empty);
            }

            string userData = JsonConvert.SerializeObject(user);
            return new HttpResponse(200, "OK", string.Empty)
            {
                Content = userData
            };
        }

        [Post]
        public HttpResponse UpdateUser(string username)
        {
            User user = userRepository.GetUserByUsername(username);
            if (user != null)
            {
                // update user data here
                bool success = userRepository.UpdateUser(user);
                if (success)
                {
                    return new HttpResponse(200, "User sucessfully updated.", string.Empty);
                }
                else
                {
                    return new HttpResponse(500, "User was found, but there was an intern error", string.Empty);
                }
            }

            return new HttpResponse(404, "User not found.", string.Empty);
        }

        private readonly IUserRepository userRepository;
    }
}
