
namespace MTCG.Models
{
    public abstract class Card
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Damage { get; set; }

        /// <summary>
        /// ElemnentTyp könnte in die ElementEffect klasse
        /// </summary>
        public ElementTyp ElementTyp { get; set; }

        public virtual Card BattleAgainst(Card other)
        {
            double card1DamageMultiplier = GetDamageMulitplier(this.ElementTyp, other.ElementTyp);
            double card2DamageMultiplier = GetDamageMulitplier(other.ElementTyp, this.ElementTyp);

            double card1Damage = this.Damage * card1DamageMultiplier;
            double card2Damage = this.Damage * card2DamageMultiplier;

            if (card1Damage > card2Damage)
            {
                // monster 1 slayed monster 2
                return this;
            }
            else if (card1Damage < card2Damage)
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

        private double GetDamageMulitplier(ElementTyp attack, ElementTyp defend)
        {
            switch (attack)
            {
                case ElementTyp.Fire:
                    if (defend == ElementTyp.Water)
                    {
                        return 0.5;
                    }
                    else if (defend == ElementTyp.Normal)
                    {
                        return 2;
                    }
                    return 1;

                case ElementTyp.Water:
                    if (defend == ElementTyp.Fire)
                    {
                        return 2;
                    }
                    else if (defend == ElementTyp.Normal)
                    {
                        return 0.5;
                    }
                    return 1;

                case ElementTyp.Normal:
                    if (defend == ElementTyp.Fire)
                    {
                        return 0.5;
                    }
                    else if (defend == ElementTyp.Water)
                    {
                        return 2;
                    }
                    return 1;

                default: throw new NotImplementedException();
            }
        }
    }
}