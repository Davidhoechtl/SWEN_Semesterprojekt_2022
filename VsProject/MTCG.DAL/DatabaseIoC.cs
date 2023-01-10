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
            IDbConnection dbConnection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=test;Database=MonsterCardTradingGame");
            builder.RegisterInstance(dbConnection);

            builder.RegisterTypes(
               typeof(NpgSqlQueryDatabase)
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