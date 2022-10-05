﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTCG.Models
{
    public abstract class Card
    {
        public string Name { get; set; }
        public double Damage { get; set; }

        /// <summary>
        /// ElemnentTyp könnte in die ElementEffect klasse
        /// </summary>
        public ElementTyp ElementTyp { get; set; }

        public abstract Card BattleAgainst(Card other);
    }
}