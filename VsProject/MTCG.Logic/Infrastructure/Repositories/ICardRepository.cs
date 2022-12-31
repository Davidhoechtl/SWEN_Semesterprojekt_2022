﻿using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Logic.Infrastructure.Repositories
{
    public interface ICardRepository
    {
        IEnumerable<Card> GetAllAvailableCards();
        IEnumerable<Card> GetUserCards(int user_Id);
        Card GetCardByName(string name);
    }
}
