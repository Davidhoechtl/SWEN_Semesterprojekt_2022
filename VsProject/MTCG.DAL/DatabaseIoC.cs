using Autofac;
using Npgsql;
using System.Data;

namespace MTCG.DAL
{
    public class DatabaseIoC : Autofac.Module
    {
        public DatabaseIoC(ContainerBuilder builder)
        {
            this.builder = builder;
        }
        
        public void Load()
        {
            IDbConnection dbConnection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=dividi1212;Database=MonsterCardTradingGame");
            builder.RegisterInstance(dbConnection);

            builder.RegisterTypes(
               typeof(NpSqlQueryDatabase)
            )
            .SingleInstance()
            .AsImplementedInterfaces();

            builder.RegisterTypes(
               typeof(UnitOfWorkFactory)
            )
            .SingleInstance();
        }

        private readonly ContainerBuilder builder;
    }
}