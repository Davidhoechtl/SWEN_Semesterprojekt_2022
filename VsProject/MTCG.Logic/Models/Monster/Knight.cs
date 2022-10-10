using MTCG.Logic.Models.Spells;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Logic.Models.Monster
{
    internal class Knight : MonsterCard
    {
        public override Card BattleAgainst(Card other)
        {
            if(other is WaterSpell)
            {
                // Knights sind zu schwer für Wasser spells
                // Water Spells gewinnen sofort
                return other;
            }

            return base.BattleAgainst(other);
        }
    }
}
