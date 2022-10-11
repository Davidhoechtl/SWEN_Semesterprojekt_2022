using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTCG.Models
{
    public class SpellCard : Card
    {
        public override Card BattleAgainst(Card other)
        {
            return base.BattleAgainst(other);
        }
    }
}