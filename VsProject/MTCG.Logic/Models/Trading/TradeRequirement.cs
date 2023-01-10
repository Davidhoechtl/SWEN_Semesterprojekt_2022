using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Logic.Models.Trading
{
    public abstract class TradeRequirement
    {
        public abstract bool MeetsRequirement(Card card);
    }
}
