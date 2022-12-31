

namespace MTCG.Logic.Infrastructure.Repositories
{
    using MTCG.DAL;
    using MTCG.Models;
    using Npgsql;

    public class CardRepository : ICardRepository
    {
        public CardRepository(IQueryDatabase database)
        {
            this.database = database;
        }

        public IEnumerable<Card> GetAllAvailableCards()
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

        public IEnumerable<Card> GetUserCards(int user_Id)
        {
            string sqlStatement = 
                @"SELECT cards.* 
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

        public Card GetCardByName(string name)
        {
            throw new NotImplementedException();
        }

        private Card GetCardFromReader(NpgsqlDataReader reader)
        {
            Card card = null;

            if (reader.IsOnRow)
            {
                char cardType = reader.GetChar(3);

                if(cardType == 'M')
                {
                    card = new MonsterCard()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Damage = reader.GetDouble(2),
                        ElementTyp = ConvertCharToElementTyp(reader.GetChar(4))
                    };
                }
                else
                {
                    card = new SpellCard()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Damage = reader.GetDouble(2),
                        ElementTyp = ConvertCharToElementTyp(reader.GetChar(4))
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

  

        private readonly IQueryDatabase database;
    }
}
