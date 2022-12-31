using System;

namespace MonsterTradingCardGame_Hoechtl.Models
{
    internal class SessionContext
    {
        public int? UserId { get; set; }
        public SessionKey SessionKey { get; set; }
    }
}
