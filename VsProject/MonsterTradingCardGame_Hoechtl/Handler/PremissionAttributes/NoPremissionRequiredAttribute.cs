
namespace MonsterTradingCardGame_Hoechtl.Handler.PremissionAttributes
{
    using MonsterTradingCardGame_Hoechtl.Models;

    internal class NoPremissionRequiredAttribute : PermissionAttribute
    {
        public override Permission RequiredPermission => Permission.None;
    }
}
