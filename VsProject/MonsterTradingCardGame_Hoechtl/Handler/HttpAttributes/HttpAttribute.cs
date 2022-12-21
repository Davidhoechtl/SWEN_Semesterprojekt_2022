using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame_Hoechtl.Handler.HttpAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    internal abstract class HttpAttribute : Attribute
    {
        public abstract Infrastructure.HttpMethod Method { get; }
    }
}
