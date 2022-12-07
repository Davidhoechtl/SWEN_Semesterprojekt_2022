using Autofac;
using MonsterTradingCardGame_Hoechtl.Handler;
using MTCG.Models;
using System.Xml.Schema;

namespace MonsterTradingCardGame_Hoechtl
{
    class Program
    {
        private static IContainer container;
        public static void Main(string[] args)
        {
            // Initialisieren des Container Builders
            ContainerBuilder containerBuilder = new();
            ContainerBootstrapper bootstrapper = new(containerBuilder);

            // Registrierung der jeweiligen Instancen
            bootstrapper.Register();

            // build des Containers
            container = bootstrapper.Build();

            Server accessPoint = container.Resolve<Server>();
            accessPoint.Start();
        }
    }
}
