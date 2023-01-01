using MTCG.DAL;
using MTCG.Logic.Infrastructure.Repositories;
using MTCG.Models;

namespace MTCG.Infrastructure
{
    public class PackageFactory
    {
        private Card[] avialableCards;
       
        public PackageFactory(IQueryDatabase database, ICardRepository cardRepository)
        {
            avialableCards = cardRepository.GetAllAvailableCards(database).ToArray();
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

            List<Card> randomCards = avialableCards
                .OrderBy(x => Guid.NewGuid())
                .Take(cardCount)
                .ToList();

            Package package = new() { 
                Price = price,
                CardIds = randomCards,
                Active = true 
            };

            return package;
        }
    }
}