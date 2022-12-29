using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTCG.Models
{
    public class User
    {
        public int Id { get; set; }
        public UserCredentials Credentials { get; set; }
        public List<Card> Stack { get; set; } = new();
        public Deck Deck { get; set; }
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