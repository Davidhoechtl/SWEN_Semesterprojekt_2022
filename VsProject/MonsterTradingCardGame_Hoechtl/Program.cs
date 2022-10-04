using Autofac;
using MonsterTradingCardGame_Hoechtl.Handler;
using MTCG.Models;

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

            UserModule test = container.Resolve<UserModule>();

            User testUser = new User()
            {
                Credentials = new UserCredentials("Test", "Test")
            };

            Console.Write(testUser.Credentials.UserName);
        }
    }
}
