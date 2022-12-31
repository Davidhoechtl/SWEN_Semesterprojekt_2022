using MTCG.Logic.Infrastructure.Repositories;
using MTCG.Models;

namespace MTCG.Infrastructure
{
    public class PackageFactory
    {
        private readonly Random random;
        private Card[] avialableCards;
       
        public PackageFactory(ICardRepository cardRepository, Random random)
        {
            this.random = random;
            avialableCards = cardRepository.GetAllAvailableCards().ToArray();
        }

        /// <summary>
        /// Creates a Package with random Cards
        /// </summary>
        /// <param name="cardCount"></param>
        /// <returns></returns>
        public Package CreatePackage(int cardCount)
        {
            if (avialableCards.Length < cardCount)
            {
                throw new Exception($"Not enough available Cards to generate Package (CardCount: {cardCount})");
            }

            Package package = new();
            for (int i = 0; i < cardCount; i++)
            {
                Card randomCard = avialableCards[random.Next(0, avialableCards.GetUpperBound(0))];
                package.Cards.Add(randomCard);
            }

            return package;
        }
    }
}