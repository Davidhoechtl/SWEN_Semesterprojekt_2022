using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTCG.Models
{
    public class Package
    {
        public List<Card> Cards { get; init; } = new();
    }
}