using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame_Hoechtl
{
    class Program
    {
        public static void Main(string[] args)
        {
            User testUser = new User()
            {
                Credentials = new UserCredentials("Test", "Test")
            };

            Console.Write(testUser.Credentials.UserName);
        }
    }
}
