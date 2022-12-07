using MonsterTradingCardGame_Hoechtl.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame_Hoechtl.Handler
{
    internal class BattleModule : IHandler
    {
        public string ModuleName => "Battle";

        public Func<string, HttpResponse> HandlerAction => throw new NotImplementedException();
    }
}
