
namespace MonsterTradingCardGame_Hoechtl.Handler
{
    using MonsterTradingCardGame_Hoechtl.Infrastructure;

    /// <summary>
    /// A Handler takes in the requested Method Path and returns a HttpResponse with the requested Data
    /// 
    /// Handler contains a ModuleName which mapps the first part of the request path
    /// The Handler then uses its HandlerAction to process the reest of the request path and executes the bussines logic
    /// </summary>
    internal interface IHandler
    {
        string ModuleName { get; }
        Func<string, HttpResponse> HandlerAction { get; }
    }
}
