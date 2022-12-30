
namespace MonsterTradingCardGame_Hoechtl.Handler.PremissionAttributes
{
    using MonsterTradingCardGame_Hoechtl.Models;

    internal class AdminPermissionRequired : PermissionAttribute
    {
        public override Permission RequiredPermission => Permission.Admin;
    }
}
