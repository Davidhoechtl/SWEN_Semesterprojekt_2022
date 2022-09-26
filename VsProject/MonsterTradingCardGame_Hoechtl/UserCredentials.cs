using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardEntwurf
{
    public class UserCredentials
    {
        public string UserName { get; init; }
        public string Password { get; init; }

        public UserCredentials( string username, string password )
        {
            UserName = username;
            Password = password;
        }
    }
}
