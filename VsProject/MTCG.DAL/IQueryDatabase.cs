

namespace MTCG.DAL
{
    using Npgsql;

    public interface IQueryDatabase
    {
        T GetItem<T>(string statement, Func<NpgsqlDataReader, T> selector, params NpgsqlParameter[] parameter);
        List<T> GetItems<T>(string statement, Func<NpgsqlDataReader, List<T>> selector, params NpgsqlParameter[] parameter);

        int? InsertAndGetLastIdentity (string statement, params NpgsqlParameter[] parameter);
        int ExecuteNonQuery(string statement, params NpgsqlParameter[] parameter);
    }
}
