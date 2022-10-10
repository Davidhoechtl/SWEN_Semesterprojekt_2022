using MTCG.Models;

namespace MTCG.Logic.Infrastructure.Repositories
{
    public class MockCardRepository : ICardRepository
    {
        private static readonly IEnumerable<Card> availableCards = new List<Card>()
        {
            new MonsterCard()
            {
                Name = "Wolve",
                Damage = 10,
                ElementTyp = ElementTyp.Normal
            },
            new MonsterCard()
            {
                Name = "Shark",
                Damage = 15,
                ElementTyp = ElementTyp.Water
            },
            new MonsterCard()
            {
                Name = "Dragon",
                Damage = 20,
                ElementTyp = ElementTyp.Fire
            },
            new MonsterCard()
            {
                Name="Mouse",
                Damage = 8,
                ElementTyp = ElementTyp.Normal
            },
            new SpellCard()
            {
                Name="Ultimate Kill",
                Damage = 100,
                ElementTyp = ElementTyp.Normal
            },
            new SpellCard()
            {
                Name = "FireBall",
                Damage = 15,
                ElementTyp = ElementTyp.Fire
            },
            new SpellCard()
            {
                Name = "WaterBall",
                Damage = 15,
                ElementTyp = ElementTyp.Water
            },
            new SpellCard()
            {
                Name = "NeedleShot",
                Damage = 8,
                ElementTyp = ElementTyp.Normal
            }
        };

        public IEnumerable<Card> GetAllAvailableCards()
        {
            return availableCards;
        }

        public Card GetCardByName(string name)
        {
            return availableCards.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
