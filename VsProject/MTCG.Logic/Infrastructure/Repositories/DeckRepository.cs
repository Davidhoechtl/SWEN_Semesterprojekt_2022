
namespace MTCG.Logic.Infrastructure.Repositories
{
    using MTCG.DAL;
    using MTCG.Models;
    using Npgsql;

    public class DeckRepository : IDeckRepository
    {
        public DeckRepository(ICardRepository cardRepository)
        {
            this.cardRepository = cardRepository;
        }

        public Deck GetUsersDeck(int userId, IQueryDatabase queryDatabase)
        {
            string sqlStatement =
                @"SELECT decks.user_id, decks.deck_id, cards.*
                  FROM decks
                  LEFT JOIN decks_cards on (decks.deck_id = decks_cards.deck_id)
                  LEFT JOIN cards on (decks_cards.card_id = cards.card_id)
                  WHERE decks.user_id = @userId";

            Deck deck = queryDatabase.GetItem<Deck>(
                sqlStatement,
                reader =>
                {
                    Deck deck = new Deck();
                    if (reader.IsOnRow)
                    {
                        deck.Id = reader.GetInt32(reader.GetOrdinal("deck_id"));
                        deck.UserId = reader.GetInt32(reader.GetOrdinal("user_id"));

                        Stack<Card> cards = new();
                        do
                        {
                            if (reader["card_id"] != DBNull.Value)
                            {
                                cards.Push(cardRepository.GetCardFromReader(reader));
                            }
                        } while (reader.Read());

                        reader.Close();
                        deck.Cards = cards;
                        return deck;
                    }

                    reader.Close();
                    return null;
                },
                new NpgsqlParameter("userId", userId)
            );

            return deck;
        }

        public bool UpdateUsersDeck(Deck deck, IUnitOfWork unitOfWork)
        {
            string deleteStatement = "DELETE FROM decks_cards WHERE deck_id = @deckId";
            unitOfWork.ExecuteNonQuery(
                deleteStatement,
                new NpgsqlParameter("deckId", deck.Id)
            );

            string insertStatement = "INSERT INTO decks_cards (deck_id, card_id) VALUES (@deckId, @cardId)";
            foreach (Card card in deck.Cards)
            {
                unitOfWork.ExecuteNonQuery(insertStatement,
                    new NpgsqlParameter("deckId", deck.Id),
                    new NpgsqlParameter("cardId", card.Id)
                );
            }

            return true;
        }

        private readonly ICardRepository cardRepository;
    }
}
