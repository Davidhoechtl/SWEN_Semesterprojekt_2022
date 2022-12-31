using MTCG.DAL;
using MTCG.Models;
using Npgsql;

namespace MTCG.Logic.Infrastructure.Repositories
{
    public class PackageRepository : IPackageRepository
    {
        public PackageRepository(IQueryDatabase database)
        {
            this.database = database;
        }

        public Package GetRandomActivePackage()
        {
            string sqlStatement =
                @"SELECT packages.*
                  FROM packages 
                  JOIN packages_cards on (packages.package_id = packages_cards.package_id)
                  WHERE Active = true 
                  LIMIT 1";

            Package package = database.GetItem<Package>(
                sqlStatement,
                ConvertPackageFromReader
            );

            return package;
        }

        public bool InsertPackage(Package package)
        {
            string sqlStatement = "INSERT INTO packages (price, active) VALUES (@price, @active) RETURNING package_id";

            int? packageId = database.InsertAndGetLastIdentity(
                sqlStatement,
                new NpgsqlParameter("price", package.Price),
                new NpgsqlParameter("active", package.Active)
            );

            if (packageId.HasValue)
            {
                foreach (int cardId in package.CardIds)
                {
                    sqlStatement = "INSERT INTO packages_cards (package_id, card_id) VALUES (@packageId, @cardId)";
                    int affectedRows = database.ExecuteNonQuery(
                        sqlStatement,
                        new NpgsqlParameter("packageId", packageId.Value),
                        new NpgsqlParameter("cardId", cardId)
                    );
                }

                return true;
            }

            return false;
        }

        public bool UpdatedPackage(Package package)
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
            List<int> packageCards = new();

            if (reader.IsOnRow)
            {
                package = new Package()
                {
                    Id = reader.GetInt32(0),
                    Price = reader.GetInt32(1),
                    Active = reader.GetBoolean(2)
                };

                packageCards.Add(reader.GetInt32(reader.GetOrdinal("card_id")));
                while (reader.Read())
                {
                    packageCards.Add(reader.GetInt32(reader.GetOrdinal("card_id")));
                }

                package.CardIds = packageCards;
            }

            reader.Close();
            return package;
        }

        private readonly IQueryDatabase database;
    }
}
