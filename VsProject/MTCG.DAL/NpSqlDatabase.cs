
namespace MTCG.DAL
{
    using Npgsql;
    using System;
    using System.Data;

    public class NpSqlDatabase : IQueryDatabase
    {
        public NpSqlDatabase(IDbConnection dbConnection)
        {
            this.dbConnection = new NpgsqlConnection(dbConnection.ConnectionString);
            this.dbConnection.Open();
        }

        public T GetItem<T>(string statement, Func<NpgsqlDataReader, T> selector, params NpgsqlParameter[] parameter)
        {
            NpgsqlCommand cmd = new NpgsqlCommand(statement, dbConnection);
            foreach(var param in parameter)
            {
                cmd.Parameters.Add(param);
            }
            cmd.Prepare();

            NpgsqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            return selector.Invoke(reader);
        }

        public List<T> GetItems<T>(string statement, Func<NpgsqlDataReader, List<T>> selector, params NpgsqlParameter[] parameter)
        {
            NpgsqlCommand cmd = new NpgsqlCommand(statement, dbConnection);
            foreach (var param in parameter)
            {
                cmd.Parameters.Add(param);
            }
            cmd.Prepare();

            return selector.Invoke(cmd.ExecuteReader());
        }

        public int ExecuteNonQuery(string statement, params NpgsqlParameter[] parameter)
        {
            NpgsqlCommand cmd = new NpgsqlCommand(statement, dbConnection);
            foreach (var param in parameter)
            {
                cmd.Parameters.Add(param);
            }
            cmd.Prepare();
            return cmd.ExecuteNonQuery();
        }

        private readonly NpgsqlConnection dbConnection;
    }
}
