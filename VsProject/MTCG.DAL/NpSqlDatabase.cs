
namespace MTCG.DAL
{
    using Npgsql;
    using System.Data;

    public class NpSqlDatabase : IDatabase
    {
        public NpSqlDatabase(IDbConnection dbConnection)
        {
            this.dbConnection = new NpgsqlConnection(dbConnection.ConnectionString);
            this.dbConnection.Open();
        }

        public NpgsqlCommand GetNpgsqlCommand(string statement)
        {
            NpgsqlCommand cmd = new NpgsqlCommand(statement, dbConnection);
            return cmd;
        }

        public NpgsqlDataReader GetNpgsqlDataReader(string statement)
        {
            NpgsqlDataReader dataReader = GetNpgsqlCommand(statement).ExecuteReader();
            return dataReader;
        }

        private readonly NpgsqlConnection dbConnection;
    }
}
