using MonsterTradingCardGame_Hoechtl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame_Hoechtl.Handler.PremissionAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    internal abstract class PermissionAttribute : Attribute
    {
        public abstract Permission RequiredPermission { get; }
    }
}
