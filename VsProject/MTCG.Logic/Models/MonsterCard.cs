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
            if(other is SpellCard)
            {
                return base.BattleAgainst(other);
            }
            else
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

        public override object Clone()
        {
            return new MonsterCard()
            {
                Id = this.Id,
                Name = this.Name,
                Damage = this.Damage,
                ElementTyp = this.ElementTyp
            };
        }

        public override bool Equals(Card other)
        {
            return
                this.Id == other.Id &&
                this.Name == other.Name &&
                this.Damage == other.Damage &&
                this.ElementTyp == other.ElementTyp;
        }
    }
}