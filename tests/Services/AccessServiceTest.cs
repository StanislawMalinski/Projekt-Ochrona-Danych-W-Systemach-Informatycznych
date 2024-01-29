using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Configuration;
using projekt.Db.Repository.Interfaces;
using projekt.Db.Repository;
using projekt.Services;
using projekt.Services.Interfaces;

namespace Tests.Services
{
    public class AccessServiceTest
    {
        private Mock<ITimeOutRepository> _timeoutsRepositoryMock;
        private Mock<IActivityRepository> _activityRepositoryMock;
        private IConfiguration _configurationMock;
        private AccessService _accessService;

        private static string allowedOrigin = "http://example.com";
        private static int timeOutInMinutes = 5;
        private static int lastNMinutes = 5;
        private static int allowedCount = 5;


        [SetUp]
        public void Setup()
        {
            _timeoutsRepositoryMock = new Mock<ITimeOutRepository>();
            _activityRepositoryMock = new Mock<IActivityRepository>();
            var inMemorySettings = new Dictionary<string, string> {
                {"ClassConfig:AccessService:AllowedTimeSpanInMinutes", "" + lastNMinutes},
                {"ClassConfig:AccessService:TimeOutInMinutes", "" + timeOutInMinutes},
                {"ClassConfig:AccessService:AllowedCount", "" + allowedCount},
            };
            _configurationMock = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _accessService = new AccessService(
                _timeoutsRepositoryMock.Object,
                _activityRepositoryMock.Object,
                _configurationMock
            );
        }

        [Test]
        public void ShouldReplay_ReturnsTrue_WhenOriginIsAllowedAndCountNumberIsNotExcided()
        {
            // Arrange
            _activityRepositoryMock.Setup(x => x.GetAcitivityCountForLastNMinutes(allowedOrigin, lastNMinutes)).Returns(allowedCount - 1);
            // Act
            bool result = _accessService.ShouldReplay(allowedOrigin);
            // Assert
            Assert.IsTrue(result);
            _timeoutsRepositoryMock.Verify(x => x.setTimeOut(allowedOrigin, timeOutInMinutes), Times.Never);
        }


        [Test]
        public void ShouldReplay_ReturnsFalse_WhenCountNumberIsExcided()
        {
            // Arrange
            _timeoutsRepositoryMock.Setup(x => x.isTimeOut(allowedOrigin)).Returns(false);
            _activityRepositoryMock.Setup(x => x.GetAcitivityCountForLastNMinutes(allowedOrigin, lastNMinutes)).Returns(allowedCount + 2);
            // Act
            bool result = _accessService.ShouldReplay(allowedOrigin);
            // Assert
            Assert.IsFalse(result);
            _timeoutsRepositoryMock.Verify(x => x.setTimeOut(allowedOrigin, timeOutInMinutes), Times.Once);
        }

        [Test]
        public void ShouldReplay_ReturnsFalse_WhenOriginIsTimedOut()
        {
            // Arrange
            _timeoutsRepositoryMock.Setup(x => x.isTimeOut(allowedOrigin)).Returns(true);
            // Act
            bool result = _accessService.ShouldReplay(allowedOrigin);
            // Assert
            Assert.IsFalse(result);
            _timeoutsRepositoryMock.Verify(x => x.setTimeOut(allowedOrigin, timeOutInMinutes), Times.Never);
        }
    }
}
