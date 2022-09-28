using Autofac;
using MTCG.Infrastructure;
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
            RegisterInfrastructure();
            RegisterRepository();
            RegisterService();
        }

        private void RegisterInfrastructure()
        {
            builder.RegisterType(
                typeof(PackageFactory)
            )
            .SingleInstance();
        }

        private void RegisterRepository()
        {
        }

        private void RegisterService()
        {
        }
    }
}
