using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Logic.Models.Monster
{
    public class Goblin : MonsterCard
    {
        public override Card BattleAgainst(Card other)
        {
            if(other is Dragon)
            {
                // Goblins cant attack dragons
                return other;
            }

            return base.BattleAgainst(other);
        }
    }
}
