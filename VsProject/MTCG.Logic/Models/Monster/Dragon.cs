using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Logic.Models.Monster
{
    internal sealed class Dragon : MonsterCard
    {
        public override Card BattleAgainst(Card other)
        {
            if(other is Goblin)
            {
                // goblins sind können Dragons nicht angreifen
                // deshalb gewinnt der Dragon
                return this;
            }

            return base.BattleAgainst(other);
        }
    }
}
