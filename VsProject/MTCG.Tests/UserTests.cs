
namespace MTCG.Tests
{
    using MTCG.DAL;
    using MTCG.Logic.Infrastructure.Repositories.MockUps;
    using Npgsql;

    internal class UserTests
    {
        [SetUp]
        public void Setup()
        {
            NpgsqlConnection mockConnection = new NpgsqlConnection();
            mockDatabase = new NpgSqlQueryDatabase(mockConnection);
        }

        [Test]
        public void Test_GetUserByUsername()
        {
            //Arrange
            IUserRepository mockRepo = new MockUserRepository();
            string username = "TestUser";

            //Act
            User found = mockRepo.GetUserByUsername(username, mockDatabase);

            //Assert
            Assert.NotNull(found);
            Assert.That(found.Credentials.UserName.Equals(username,StringComparison.Ordinal));
        }

        private IQueryDatabase mockDatabase;
    }
}
