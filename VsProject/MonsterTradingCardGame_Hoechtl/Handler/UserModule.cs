using MonsterTradingCardGame_Hoechtl.Infrastructure;
using MTCG.Logic.Infrastructure.Repositories;
using MTCG.Models;
using Newtonsoft.Json;

namespace MonsterTradingCardGame_Hoechtl.Handler
{
    internal class UserModule
    {
        private readonly IUserRepository userRepository;

        public UserModule(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public ResponseMessage RegisterUser(UserCredentials userCredentials)
        {
            bool alreadyInDatabase = userRepository.GetUserByUsername(userCredentials.UserName) != null;
            if (alreadyInDatabase)
            {
                return new ResponseMessage(409, "User with same username already registered");
            }

            // encrpyt passwort hier
            bool success = userRepository.SaveUser(userCredentials.UserName, userCredentials.Password);
            if (!success)
            {
                return new ResponseMessage(400, $"Fehler beim Speichern des Users {userCredentials.UserName}.");
            }
            return new ResponseMessage(201);
        }

        public ResponseMessage LoginUser(UserCredentials userCredentials)
        {
            User user = userRepository.GetUserByUsername(userCredentials.UserName);
            if (user != null)
            {
                // decrypt password
                if (user.Credentials.Password.Equals(userCredentials.Password, StringComparison.Ordinal))
                {
                    // retrun session Token
                    return new ResponseMessage(200, "User login successful");
                }
            }

            return new ResponseMessage(401, "Invalid username/password provided");
        }

        public ResponseMessage GetUser(string username)
        {
            User user = userRepository.GetUserByUsername(username);
            if (user == null)
            {
                return new ResponseMessage(404, "User not found.");
            }

            string userData = JsonConvert.SerializeObject(user);
            return new ResponseMessage(200)
            {
                Content = userData
            };
        }

        public ResponseMessage UpdateUser(string username, string test)
        {
            User user = userRepository.GetUserByUsername(username);
            if (user != null)
            {
                // update user data here
                bool success = userRepository.UpdateUser(user);
                if (success)
                {
                    return new ResponseMessage(200, "User sucessfully updated.");
                }
                else
                {
                    return new ResponseMessage(500, "User was found, but there was an intern error");
                }
            }

            return new ResponseMessage(404, "User not found.");
        }
    }
}
