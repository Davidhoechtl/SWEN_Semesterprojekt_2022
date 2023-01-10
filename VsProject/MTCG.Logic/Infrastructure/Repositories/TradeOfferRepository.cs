using MTCG.DAL;
using MTCG.Logic.Models.Trading;
using MTCG.Models;
using Npgsql;

namespace MTCG.Logic.Infrastructure.Repositories
{
    public class TradeOfferRepository : ITradeOfferRepository
    {
        public TradeOfferRepository(CardFactory cardFactory)
        {
            this.cardFactory = cardFactory;
        }

        public IEnumerable<TradingOffer> GetAllTradeOffers(IQueryDatabase queryDatabase)
        {
            string sqlStatement =
                @"SELECT * 
                  FROM trade_offers
                  JOIN cards ON (trade_offers.offered_card_id = cards.card_id)";

            return queryDatabase.GetItems(
                sqlStatement,
                reader =>
                {
                    List<TradingOffer> offers = new();

                    while (reader.Read())
                    {
                        offers.Add(ConvertTradingOfferFromDataReader(reader));
                    }

                    reader.Close();
                    return offers;
                }
            );
        }

        public List<TradingOffer> GetTradeOffersBySellerId(int sellerId, IQueryDatabase queryDatabase)
        {
            string sqlStatement = "SELECT * FROM trade_offers WHERE seller_id = @sellerId";

            return queryDatabase.GetItems(
                sqlStatement,
                reader =>
                {
                    List<TradingOffer> offers = new();

                    while (reader.Read())
                    {
                        offers.Add(ConvertTradingOfferFromDataReader(reader));
                    }

                    reader.Close();
                    return offers;
                },
                new NpgsqlParameter("sellerId", sellerId)
            );
        }

        public TradingOffer GetTradingOfferById(int tradeId, IQueryDatabase queryDatabase)
        {
            string sqlStatement = "SELECT * FROM trade_offers WHERE trade_id = @tradeId";

            return queryDatabase.GetItem(
                sqlStatement,
                reader =>
                {
                    TradingOffer offer = null;
                    if (reader.IsOnRow)
                    {
                        offer = ConvertTradingOfferFromDataReader(reader);
                    }

                    reader.Close();
                    return offer;
                },
                new NpgsqlParameter("tradeId", tradeId)
            );
        }

        public bool InsertTradeOffer(TradingOffer tradeOffer, IQueryDatabase queryDatabase)
        {
            string sqlStatement =
              @"INSERT INTO trade_offers (offered_card_id, seller_id, type_requirement, damage_requirement, category_requirement, element_requirement, active)
                VALUES(@offeredCardId, @sellerId, @typeRequirement, @damageRequirement, @categoryRequirement, @elementRequirement, @active)";

            char? cardTypeRequirement = GetCardTypeRequirementValue(tradeOffer);
            double? damageRequirement = tradeOffer.GetTradRequirement<DamageRequirement>()?.MinimumDamage;
            ElementTyp? elementRequirement = tradeOffer.GetTradRequirement<ElementTypRequirement>()?.ElementTyp;
            string categoryRequirement = tradeOffer.GetTradRequirement<CardCategoryRequirement>()?.CardType?.Name;

            int deltaRows = queryDatabase.ExecuteNonQuery(
                sqlStatement,
                new NpgsqlParameter("offeredCardId", tradeOffer.Card.Id),
                new NpgsqlParameter("sellerId", tradeOffer.SellerId),
                new NpgsqlParameter("typeRequirement", cardTypeRequirement),
                new NpgsqlParameter("damageRequirement", damageRequirement),
                new NpgsqlParameter("elementRequirement", elementRequirement),
                new NpgsqlParameter("categoryRequirement", categoryRequirement),
                new NpgsqlParameter("active", tradeOffer.Active)
            );

            return deltaRows != 0;
        }

        public bool UpdateTradeOffer(TradingOffer tradeOffer, IQueryDatabase queryDatabase)
        {
            string sqlStatement = "UPDATE trade_offers SET active = @active, buyer_id = @buyerId WHERE trade_id = @tradeId";

            int deltaRows = queryDatabase.ExecuteNonQuery(
                sqlStatement,
                new NpgsqlParameter("buyerId", tradeOffer.BuyerId.Value),
                new NpgsqlParameter("active", tradeOffer.Active)
            );

            return deltaRows != 0;
        }

        private TradingOffer ConvertTradingOfferFromDataReader(NpgsqlDataReader reader)
        {
            TradingOffer offer = null;

            if (reader.IsOnRow)
            {
                List<TradeRequirement> tradeRequirements = new();
                if (!reader.IsDBNull(reader.GetOrdinal("type_requirement")))
                {
                    char typeRequirement = reader.GetChar(reader.GetOrdinal("type_requirement"));
                    tradeRequirements.Add(GetCardTypeRequirementFromValue(typeRequirement));
                }
                if (!reader.IsDBNull(reader.GetOrdinal("damage_requirement")))
                {
                    double damageRequirement = reader.GetDouble(reader.GetOrdinal("damage_requirement"));
                    tradeRequirements.Add(new DamageRequirement(damageRequirement));
                }
                if (!reader.IsDBNull(reader.GetOrdinal("element_requirement")))
                {
                    char elementRequirement = reader.GetChar(reader.GetOrdinal("element_requirement"));
                    ElementTyp convertedElementType = cardFactory.ConvertCharToElementTyp(elementRequirement);
                    tradeRequirements.Add(new ElementTypRequirement(convertedElementType));
                }
                if (!reader.IsDBNull(reader.GetOrdinal("category_requirement")))
                {
                    string categoryRequirement = reader.GetString(reader.GetOrdinal("category_requirement"));
                    Type convertedCardType = GetCardTypeByCategory(categoryRequirement);
                    tradeRequirements.Add(new CardCategoryRequirement(convertedCardType));
                }

                int? buyerId = null;
                if (!reader.IsDBNull(reader.GetOrdinal("buyer_id")))
                {
                    buyerId = reader.GetInt32(reader.GetOrdinal("buyer_id"));
                }

                offer = new TradingOffer(
                   id: reader.GetInt32(reader.GetOrdinal("trade_id")),
                   sellerId: reader.GetInt32(reader.GetOrdinal("seller_id")),
                   buyerId: buyerId,
                   card: cardFactory.GetCardFromDataReader(reader),
                   tradeRequirements: tradeRequirements,
                   active: reader.GetBoolean(reader.GetOrdinal("active"))
                );
            }

            return offer;
        }

        private TradeRequirement GetCardTypeRequirementFromValue(char type)
        {
            if (type == 'M')
            {
                return new CardTypRequirement<MonsterCard>();
            }
            else
            {
                return new CardTypRequirement<SpellCard>();
            }
        }

        private char? GetCardTypeRequirementValue(TradingOffer offer)
        {
            if (offer.GetTradRequirement<CardTypRequirement<MonsterCard>>() != null)
            {
                return 'M';
            }
            else if (offer.GetTradRequirement<CardTypRequirement<SpellCard>>() != null)
            {
                return 'S';
            }

            return null;
        }

        private Type GetCardTypeByCategory(string category)
        {
            return cardFactory.CardTypes.First(type => type.Name == category);
        }

        private readonly CardFactory cardFactory;
    }
}
