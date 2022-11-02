using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame_Hoechtl.Models
{
    internal class Session
    {
        public string SessionToken { get; }
        public Role Role { get; }
        public User User { get; }
    }
}
