using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTCG.Models
{
    public class SpellCard : Card, IHasSpell
    {

        public void Cast()
        {
            throw new NotImplementedException();
        }
    }
}