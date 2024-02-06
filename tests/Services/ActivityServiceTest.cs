using NUnit.Framework;
using Moq;
using projekt.Models.Enums;
using projekt.Models.Dtos;
using projekt.Db.Repository.Interfaces;
using projekt.Db.Repository;
using projekt.Services.Interfaces;
using projekt.Services;

namespace tests.Services
{
    [TestFixture]
    public class ActivityServiceTests
    {
        private Mock<IActivityRepository> _activityRepositoryMock;
        private Mock<IAccountRepository> _accountRepositoryMock;
        private Mock<IDebugSerivce> _debugServiceMock;
        private ActivityService _activityService;

        [SetUp]
        public void Setup()
        {
            _activityRepositoryMock = new Mock<IActivityRepository>();
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _debugServiceMock = new Mock<IDebugSerivce>();
            _activityService = new ActivityService(_activityRepositoryMock.Object, _accountRepositoryMock.Object, _debugServiceMock.Object);
        }

        [Test]
        public void LogActivity_WithOrigin_ShouldCallRepositories()
        {
            // Arrange
            var activityType = ActivityType.Login;
            var associatedEmailOrAccountNumber = "test@test.com";
            var origin = "origin";
            var success = true;
            _accountRepositoryMock.Setup(ar => ar.GetAccount(associatedEmailOrAccountNumber)).Returns(new Account { Email = associatedEmailOrAccountNumber });
            // Act
            _activityService.LogActivity(activityType, associatedEmailOrAccountNumber, origin, success);

            // Assert
            _debugServiceMock.Verify(ds => ds.LogActivity(It.IsAny<Activity>()), Times.Once);
            _activityRepositoryMock.Verify(ar => ar.LogActivity(It.IsAny<Activity>()), Times.Once);
        }

        [Test]
        public void LogActivity_WithoutOrigin_ShouldCallRepositories()
        {
            // Arrange
            var activityType = ActivityType.Login;
            var associatedEmailOrAccountNumber = "test@test.com";
            var success = true;
            _accountRepositoryMock.Setup(ar => ar.GetAccount(associatedEmailOrAccountNumber)).Returns(new Account { Email = associatedEmailOrAccountNumber });
            // Act
            _activityService.LogActivity(activityType, associatedEmailOrAccountNumber, success);

            // Assert
            _debugServiceMock.Verify(ds => ds.LogActivity(It.IsAny<Activity>()), Times.Once);
            _activityRepositoryMock.Verify(ar => ar.LogActivity(It.IsAny<Activity>()), Times.Once);
        }
    }
}