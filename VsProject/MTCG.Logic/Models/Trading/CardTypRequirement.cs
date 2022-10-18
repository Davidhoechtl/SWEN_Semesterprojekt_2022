﻿using MTCG.Models;

namespace MTCG.Logic.Models.Trading
{
    public class CardTypRequirement<T> : TradeRequirement
        where T : Card
    {
        protected override bool MeetsRequirement(Card card)
        {
            Type requiredType = typeof(T);
            return card.GetType() == requiredType;
        }
    }
}
