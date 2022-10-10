using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Logic.Models.Monster
{
    public class Wizzard : MonsterCard
    {
        public override Card BattleAgainst(Card other)
        {
            if(other is Ork)
            {
                // Wizzards können Orks verzaubern weshlab sie nicht angreifen können
                return this;
            }

            return base.BattleAgainst(other);
        }
    }
}
