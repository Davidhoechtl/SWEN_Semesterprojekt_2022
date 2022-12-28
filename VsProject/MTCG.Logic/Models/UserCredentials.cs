using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    public class UserCredentials
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public UserCredentials( string username, string password )
        {
            UserName = username;
            Password = password;
        }
    }
}
