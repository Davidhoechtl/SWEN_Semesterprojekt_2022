
namespace MTCG.Models
{
    public class SpellCard : Card
    {
        public override Card BattleAgainst(Card other)
        {
            return base.BattleAgainst(other);
        }

        public override object Clone()
        {
            return new SpellCard()
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