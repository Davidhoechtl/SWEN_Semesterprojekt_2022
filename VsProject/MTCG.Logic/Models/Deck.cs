using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTCG.Models
{
    public class Deck
    {
        public Stack<Card> Cards { get; set; } = new();
    }
}