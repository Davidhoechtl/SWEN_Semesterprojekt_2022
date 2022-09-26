using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterTradingCardEntwurf
{
    public class User
    {
        public UserCredentials Credentials { get; set; }
        public List<Card> Stack { get; set; }
        public int Coins { get; set; }

        public Deck SelectDeck()
        {
            // User wählt aus seinem Deck eine bestimmte Anzahl an Karten aus
            return null;
        }

        /// <summary>
        /// Die Battle Methode würde später in ein UI Backend verschoben werden
        /// </summary>
        public void Battle()
        {

        }

        /// <summary>
        /// Die Trade Methode würde später in ein UI Backend verschoben werden
        /// </summary>
        public void Trade()
        {

        }
    }
}