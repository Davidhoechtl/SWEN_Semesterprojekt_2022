using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTCG.Models
{
    public interface IHasSpell
    {
        public abstract void Cast();
    }
}