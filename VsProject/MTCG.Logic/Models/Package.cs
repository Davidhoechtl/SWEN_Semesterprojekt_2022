﻿
namespace MTCG.Models
{
    using System.Collections.Generic;

    public class Package
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public int Price { get; set; }
        public List<Card> CardIds { get; set; } = new();
    }
}