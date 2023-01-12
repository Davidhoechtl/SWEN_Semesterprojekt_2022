
namespace MTCG.Tests
{
    using MTCG.DAL;
    using MTCG.Logic.Infrastructure.Repositories.MockUps;
    using Npgsql;

    internal class UserTests
    {
        [Test]
        public void Test_GetUserByUsername()
        {
            //Arrange
            IUserRepository mockRepo = new MockUserRepository();
            string username = "TestUser";

            //Act
            User found = mockRepo.GetUserByUsername(username, null);

            //Assert
            Assert.NotNull(found);
            Assert.That(found.Credentials.UserName.Equals(username,StringComparison.Ordinal));
        }
    }
}
