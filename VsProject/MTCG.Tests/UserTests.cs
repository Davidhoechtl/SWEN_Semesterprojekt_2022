﻿
namespace MTCG.Tests
{
    internal class UserTests
    {
        [Test]
        public void Test_GetUserByUsername()
        {
            //Arrange
            IUserRepository mockRepo = new MockUserRepository();
            string username = "TestUser";

            //Act
            User found = mockRepo.GetUserByUsername(username);

            //Assert
            Assert.NotNull(found);
            Assert.Equals(found.Credentials.UserName, username);
        }
    }
}