using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTCG.Models
{
    public class Deck : ICloneable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public Stack<Card> Cards { get; set; } = new();

        public object Clone()
        {
            return new Deck()
            {
                Id = Id,
                UserId = UserId,
                Cards = CloneCards()
            };
        }

        private Stack<Card> CloneCards()
        {
            Stack<Card> stack = new Stack<Card>();
            foreach(Card card in Cards)
            {
                stack.Push((Card)card.Clone());
            }
            return stack;
        }
    }
}