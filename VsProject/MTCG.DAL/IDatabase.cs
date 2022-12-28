

namespace MTCG.DAL
{
    using Npgsql;

    public interface IDatabase
    {
        public NpgsqlCommand GetNpgsqlCommand(string statement);
        public NpgsqlDataReader GetNpgsqlDataReader(string statement);
    }
}
