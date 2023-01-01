using MTCG.DAL;
using MTCG.Models;
using Npgsql;

namespace MTCG.Logic.Infrastructure.Repositories
{
    public class PackageRepository : IPackageRepository
    {
        public PackageRepository( ICardRepository cardRepository)
        {
            this.cardRepository = cardRepository;
        }

        public Package GetRandomActivePackage(IQueryDatabase database)
        {
            string sqlStatement =
                @"SELECT *
                  FROM packages
                  WHERE Active = true 
                  LIMIT 1";

            Package package = database.GetItem<Package>(
                sqlStatement,
                ConvertPackageFromReader
            );

            // Can be optimized
            foreach (int cardId in GetCardIdsOfPackage(package.Id, database))
            {
                package.CardIds.Add(cardRepository.GetCardById(cardId, database));
            }

            return package;
        }

        public bool InsertPackage(Package package, IUnitOfWork database)
        {
            string sqlStatement = "INSERT INTO packages (price, active) VALUES (@price, @active) RETURNING package_id";

            int? packageId = database.InsertAndGetLastIdentity(
                sqlStatement,
                new NpgsqlParameter("price", package.Price),
                new NpgsqlParameter("active", package.Active)
            );

            if (packageId.HasValue)
            {
                foreach (Card card in package.CardIds)
                {
                    sqlStatement = "INSERT INTO packages_cards (package_id, card_id) VALUES (@packageId, @cardId)";
                    int affectedRows = database.ExecuteNonQuery(
                        sqlStatement,
                        new NpgsqlParameter("packageId", packageId.Value),
                        new NpgsqlParameter("cardId", card.Id)
                    );
                }

                return true;
            }

            return false;
        }

        public bool UpdatedPackage(Package package, IQueryDatabase database)
        {
            string sqlStatement = "UPDATE packages SET active = @active WHERE package_id = @packageId";

            int affectedRows = database.ExecuteNonQuery(
                sqlStatement,
                new NpgsqlParameter("active", package.Active),
                new NpgsqlParameter("packageId", package.Id)
            );

            return affectedRows != 0;
        }

        private Package ConvertPackageFromReader(NpgsqlDataReader reader)
        {
            Package package = null;

            if (reader.IsOnRow)
            {
                package = new Package()
                {
                    Id = reader.GetInt32(0),
                    Price = reader.GetInt32(1),
                    Active = reader.GetBoolean(2)
                };
            }

            reader.Close();
            return package;
        }

        private List<int> GetCardIdsOfPackage(int packageId, IQueryDatabase database)
        {
            string sqlStatement = "SELECT * FROM packages_cards WHERE package_id = @packageId";
            List<int> cardIds = database.GetItems(
                sqlStatement,
                reader =>
                {
                    List<int> cards = new();
                    while (reader.Read())
                    {
                        cards.Add(reader.GetInt32(reader.GetOrdinal("card_id")));
                    }

                    reader.Close();
                    return cards;
                },
                new NpgsqlParameter("packageId", packageId)
            );

            return cardIds;
        }

        private readonly ICardRepository cardRepository;
    }
}
