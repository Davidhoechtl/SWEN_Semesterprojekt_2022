

namespace MTCG.Logic.Infrastructure.Repositories
{
    using MTCG.DAL;
    using MTCG.Models;
    using Npgsql;

    public class CardRepository : ICardRepository
    {
        public Card GetCardById(int id, IQueryDatabase database)
        {
            string sqlStatement = "SELECT * FROM cards WHERE card_id = @cardId";
            return database.GetItem<Card>(
                sqlStatement,
                reader =>
                {
                    Card card = GetCardFromReader(reader);
                    reader.Close();
                    return card;
                },
                new NpgsqlParameter("cardId", id)
            );
        }

        public IEnumerable<Card> GetAllAvailableCards(IQueryDatabase database)
        {
            string sqlStatement = "SELECT * from cards";

            List<Card> cards = database.GetItems<Card>(
                sqlStatement,
                reader =>
                {
                    List<Card> cards = new List<Card>();
                    while (reader.Read())
                    {
                        Card next = GetCardFromReader(reader);
                        if (next != null)
                        {
                            cards.Add(next);
                        }
                    }
                    reader.Close();
                    return cards;
                }
            );

            return cards;
        }

        public IEnumerable<Card> GetUserCards(int user_Id, IQueryDatabase database)
        {
            string sqlStatement = 
                @"SELECT * 
                  FROM users
                  JOIN users_cards ON (users.user_id = users_cards.user_id)
                  JOIN cards       ON (users_cards.card_id = cards.card_id)
                  WHERE users.user_id = @userId";

            List<Card> cards = database.GetItems<Card>(
                sqlStatement,
                reader =>
                {
                    List<Card> cards = new List<Card>();
                    while (reader.Read())
                    {
                        int count = reader.GetInt32(reader.GetOrdinal("count"));

                        for (int i = 0; i < count; i++)
                        {
                            Card next = GetCardFromReader(reader);
                            if (next != null)
                            {
                                cards.Add(next);
                            }
                        }
                    }
                    reader.Close();
                    return cards;
                },
                new NpgsqlParameter("userId", user_Id)
            );

            return cards;
        }

        public Card GetCardByName(string name, IQueryDatabase database)
        {
            throw new NotImplementedException();
        }

        private Card GetCardFromReader(NpgsqlDataReader reader)
        {
            Card card = null;

            if (reader.IsOnRow)
            {
                char cardType = reader.GetChar(reader.GetOrdinal("card_type"));

                if(cardType == 'M')
                {
                    card = new MonsterCard()
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("card_id")),
                        Name = reader.GetString(reader.GetOrdinal("name")),
                        Damage = reader.GetDouble(reader.GetOrdinal("damage")),
                        ElementTyp = ConvertCharToElementTyp(reader.GetChar(reader.GetOrdinal("element_type")))
                    };
                }
                else
                {
                    card = new SpellCard()
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("card_id")),
                        Name = reader.GetString(reader.GetOrdinal("name")),
                        Damage = reader.GetDouble(reader.GetOrdinal("damage")),
                        ElementTyp = ConvertCharToElementTyp(reader.GetChar(reader.GetOrdinal("element_type")))
                    };
                }  
            }

            return card;
        }

        private ElementTyp ConvertCharToElementTyp(char type)
        {
            return type switch
            {
                'N' => ElementTyp.Normal,
                'F' => ElementTyp.Fire,
                'W' => ElementTyp.Water,
                _ => throw new Exception($"Unrecognized Elementype {type}")
            };
        }
    }
}
