
namespace MonsterTradingCardGame_Hoechtl.Handler
{
    using MonsterTradingCardGame_Hoechtl.Infrastructure;
    using System;

    internal class TradingModule : IHandler
    {
        public string ModuleName => "Trade";

        public Func<string, HttpResponse> HandlerAction => throw new NotImplementedException();
    }
}
