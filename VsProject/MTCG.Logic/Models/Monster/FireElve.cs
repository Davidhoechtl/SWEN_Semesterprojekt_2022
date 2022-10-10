using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Logic.Models.Monster
{
    public class FireElve : MonsterCard
    {
        public override Card BattleAgainst(Card other)
        {
            if(other is Dragon)
            {
                // FireElves kennen die Attacken der Dragons
                // Deshalb gewinnt der FireElve
                return this;
            }
            return base.BattleAgainst(other);
        }
    }
}
