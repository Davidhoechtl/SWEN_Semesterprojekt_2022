using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Logic.Models.Monster
{
    internal class Kraken : MonsterCard
    {
        public override Card BattleAgainst(Card other)
        {
            if(other is SpellCard)
            {
                // Kraken sind immun gegen spells
                return this;
            }

            return base.BattleAgainst(other);
        }
    }
}
