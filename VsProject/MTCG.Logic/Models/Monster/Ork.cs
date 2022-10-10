using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Logic.Models.Monster
{
    public class Ork : MonsterCard
    {
        public override Card BattleAgainst(Card other)
        {
            if(other is Wizzard)
            {
                // wizzard control orks
                return other;
            }

            return base.BattleAgainst(other);
        }
    }
}
