using MTCG.DAL;
using MTCG.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Logic.Infrastructure.Repositories
{
    public interface ICardRepository
    {
        Card GetCardById(int id, IQueryDatabase database);
        IEnumerable<Card> GetAllAvailableCards(IQueryDatabase database);
        IEnumerable<Card> GetUserCards(int user_Id, IQueryDatabase database);
        Card GetCardByName(string name, IQueryDatabase database);

        Card GetCardFromReader(NpgsqlDataReader reader);
    }
}
