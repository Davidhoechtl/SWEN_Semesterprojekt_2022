using Autofac;
using MonsterTradingCardGame_Hoechtl.Handler;
using MTCG.Infrastructure;
using MTCG.Logic.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame_Hoechtl
{
    internal class ContainerBootstrapper
    {
        private readonly ContainerBuilder builder;

        public ContainerBootstrapper(ContainerBuilder builder)
        {
            this.builder = builder;
        }

        public IContainer Build()
        {
            return builder.Build();
        }

        public void Register()
        {
            RegisterRepository();
            RegisterModules();
            RegisterInfrastructure();
        }

        private void RegisterInfrastructure()
        {
            builder.RegisterTypes(
                typeof(PackageFactory),
                typeof(HandlerMethodResolver),
                typeof(Server)
            )
            .SingleInstance();
        }

        private void RegisterRepository()
        {
            // Muss gegen die echten repositories ausgetauscht werden
            builder.RegisterTypes(
                typeof(MockUserRepository),
                typeof(MockCardRepository)
            )
            .SingleInstance()
            .AsImplementedInterfaces();
        }

        private void RegisterModules()
        {
            builder.RegisterTypes(
                typeof(BattleModule),
                typeof(TradingModule),
                typeof(UserModule)
            )
            .SingleInstance()
            .AsImplementedInterfaces();
        }
    }
}
