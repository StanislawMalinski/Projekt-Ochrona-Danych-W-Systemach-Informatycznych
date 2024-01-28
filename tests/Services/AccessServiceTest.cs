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
        private static int allowedCount = 5;


        [SetUp]
        public void Setup()
        {
            _timeoutsRepositoryMock = new Mock<ITimeOutRepository>();
            _activityRepositoryMock = new Mock<IActivityRepository>();
            var inMemorySettings = new Dictionary<string, string> {
                {"AccessService:TimeOutInMinutes", "" + timeOutInMinutes},
                {"AccessService:AllowedCount", "" + allowedCount},
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
        public void ShouldReplay_ReturnsTrue_WhenOriginIsAllowed()
        {
            // Arrange
            _activityRepositoryMock.Setup(x => x.GetAcitivityCountForLastNMinutes(allowedOrigin, timeOutInMinutes)).Returns(4);
            // Act
            bool result = _accessService.ShouldReplay(allowedOrigin);
            // Assert
            Assert.IsTrue(result);
        }


        [Test]
        public void ShouldReplay_ReturnsFals_WhenCountNumberIsExcided()
        {
            // Arrange
            _activityRepositoryMock.Setup(x => x.GetAcitivityCountForLastNMinutes(allowedOrigin, timeOutInMinutes)).Returns(6);
            // Act
            bool result = _accessService.ShouldReplay(allowedOrigin);
            // Assert
            Assert.IsFalse(result);
            _timeoutsRepositoryMock.Verify(x => x.setTimeOut(allowedOrigin, timeOutInMinutes), Times.Once);
        }
    }
}
