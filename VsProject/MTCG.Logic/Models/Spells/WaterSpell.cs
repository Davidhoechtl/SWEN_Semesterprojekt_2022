using MTCG.Logic.Models.Monster;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Logic.Models.Spells
{
    public class WaterSpell : SpellCard
    {
        public override Card BattleAgainst(Card other)
        {
            if(other is Knight)
            {
                // Knights ertrinken bei WaterSpells
                return this;
            }
            else if(other is Kraken)
            {
                // Kranken is immune against spells
                return other;
            }

            return base.BattleAgainst(other);
        }
    }
}
