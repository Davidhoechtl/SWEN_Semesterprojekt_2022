using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTCG.Models
{
    public abstract class SpellCard : Card, IHasSpell
    {

        public void Cast()
        {
            throw new NotImplementedException();
        }
    }
}