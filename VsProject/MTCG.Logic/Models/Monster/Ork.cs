using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Logic.Models.Monster
{
    internal class Ork : MonsterCard
    {
        public override Card BattleAgainst(Card other)
        {
            return base.BattleAgainst(other);
        }
    }
}
