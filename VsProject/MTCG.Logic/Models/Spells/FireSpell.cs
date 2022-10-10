using MTCG.Logic.Models.Monster;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Logic.Models.Spells
{
    public class FireSpell : SpellCard
    {
        public override Card BattleAgainst(Card other)
        {
            if (other is Kraken)
            {
                //Kraken ist immune gegen spells
                return other;
            }

            return base.BattleAgainst(other);
        }
    }
}
