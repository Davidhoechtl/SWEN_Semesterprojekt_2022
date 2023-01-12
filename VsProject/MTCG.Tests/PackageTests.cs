using MTCG.DAL;
using MTCG.Infrastructure;
using MTCG.Logic.Infrastructure.Repositories.MockUps;
using Npgsql;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Tests
{
    internal class PackageTests
    {
        [SetUp]
        public void Setup()
        {
            MockCardRepository mockCardRepository = new();

            factory = new PackageFactory(null, mockCardRepository);
        }

        [Test]
        public void CreatePackage_Correct_CardCount()
        {
            int minItems = 5;
            int maxItems = 5;

            Package package = factory.CreatePackage(maxItems, minItems);

            Assert.That(package.CardIds.Count() == 5);
        }

        [Test]
        public void CreatePackage_IsActive()
        {
            int minItems = 5;
            int maxItems = 5;

            Package package = factory.CreatePackage(maxItems, minItems);

            Assert.IsTrue(package.Active);
        }

        PackageFactory factory;
    }
}
