using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTCG.Models
{
    public class MonsterCard : Card
    {
        public override Card BattleAgainst(Card other)
        {
            // Elementyp is not taken into account
            if (this.Damage > other.Damage)
            {
                // monster 1 slayed monster 2
                return this;
            }
            else if (this.Damage < other.Damage)
            {
                // monster 2 slayed monster 1
                return other;
            }
            else
            {
                // Draw
                return null;
            }
        }
    }
}