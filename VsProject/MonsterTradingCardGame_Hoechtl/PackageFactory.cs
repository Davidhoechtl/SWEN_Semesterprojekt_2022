using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterTradingCardEntwurf
{
    public class PackageFactory
    {
        private Card[] avialableCards;

        /// <summary>
        /// Verfügbare Karten werden geladen
        /// </summary>
        public void Init()
        {
            avialableCards = new Card[]
            {

            };

            // Später sollten Karten mittels Repository aus der Datenbank ausgelesen werden
        }

        /// <summary>
        /// Creates a Package with random Cards
        /// </summary>
        /// <param name="cardCount"></param>
        /// <returns></returns>
        public Package CreatePackage(int cardCount)
        {
            if(avialableCards.Length < cardCount)
            {
                throw new Exception($"Not enough available Cards to generate Package (CardCount: {cardCount})");
            }

            return null;
        }
    }
}