using MTCG.Logic.Infrastructure.Repositories;
using MTCG.Models;

namespace MTCG.Infrastructure
{
    public class PackageFactory
    {
        private Card[] avialableCards;
       
        public PackageFactory(ICardRepository cardRepository)
        {
            avialableCards = cardRepository.GetAllAvailableCards().ToArray();
        }

        /// <summary>
        /// Creates a Package with random Cards
        /// </summary>
        /// <param name="cardCount"></param>
        /// <returns></returns>
        public Package CreatePackage(int cardCount, int price)
        {
            if (avialableCards.Length < cardCount)
            {
                throw new Exception($"Not enough available Cards to generate Package (CardCount: {cardCount})");
            }

            List<int> randomCardIds = avialableCards
                .OrderBy(x => Guid.NewGuid())
                .Select(card => card.Id)
                .Take(cardCount)
                .ToList();

            Package package = new() { 
                Price = price,
                CardIds = randomCardIds,
                Active = true 
            };

            return package;
        }
    }
}