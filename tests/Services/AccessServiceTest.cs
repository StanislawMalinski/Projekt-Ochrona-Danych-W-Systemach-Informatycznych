using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Configuration;
using projekt.Db.Repository.Interfaces;
using projekt.Db.Repository;
using projekt.Services;
using projekt.Services.Interfaces;
using projekt.Models.Dtos;

namespace Tests.Services
{
    [TestFixture]
    public class AccessServiceTest
    {
        private Mock<ITimeOutRepository> _timeoutsRepositoryMock;
        private Mock<ICryptoService> _cryptoServiceMock;
        private Mock<IActivityRepository> _activityRepositoryMock;
        private Mock<ISessionRepository> _sessionRepositoryMock;
        private IConfiguration _configurationMock;
        private AccessService _accessService;

        private static string allowedOrigin = "http://example.com";
        private static int timeOutInMinutes = 5;
        private static int lastNMinutes = 5;
        private static int allowedCount = 5;

        private static Token validToken = new Token(){
            SessionId = 1,
            ExpirationDate = DateTime.Now.AddMinutes(5),
            Sign = "test"
        };

        private static Token expiredToken = new Token(){
            SessionId = 1,
            ExpirationDate = DateTime.Now.AddMinutes(-5),
            Sign = "test"
        };

        [SetUp]
        public void Setup()
        {
            _timeoutsRepositoryMock = new Mock<ITimeOutRepository>();
            _cryptoServiceMock = new Mock<ICryptoService>();
            _activityRepositoryMock = new Mock<IActivityRepository>();
            _sessionRepositoryMock = new Mock<ISessionRepository>();
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
                _cryptoServiceMock.Object,
                _activityRepositoryMock.Object,
                _sessionRepositoryMock.Object,
                _configurationMock
            );
        }

        [Test]
        public void ShouldReplayToOrigin_ReturnsTrue_WhenOriginIsAllowedAndCountNumberIsNotExcided()
        {
            // Arrange
            _activityRepositoryMock.Setup(x => x.GetAcitivityCountForLastNMinutes(allowedOrigin, lastNMinutes)).Returns(allowedCount - 1);
            // Act
            bool result = _accessService.ShouldReplayToOrigin(allowedOrigin);
            // Assert
            Assert.IsTrue(result);
            _timeoutsRepositoryMock.Verify(x => x.setTimeOut(allowedOrigin, timeOutInMinutes), Times.Never);
        }


        [Test]
        public void ShouldReplayToOrigin_ReturnsFalse_WhenCountNumberIsExcided()
        {
            // Arrange
            _timeoutsRepositoryMock.Setup(x => x.isTimeOut(allowedOrigin)).Returns(false);
            _activityRepositoryMock.Setup(x => x.GetAcitivityCountForLastNMinutes(allowedOrigin, lastNMinutes)).Returns(allowedCount + 2);
            // Act
            bool result = _accessService.ShouldReplayToOrigin(allowedOrigin);
            // Assert
            Assert.IsFalse(result);
            _timeoutsRepositoryMock.Verify(x => x.setTimeOut(allowedOrigin, timeOutInMinutes), Times.Once);
        }

        [Test]
        public void ShouldReplayToOrigin_ReturnsFalse_WhenOriginIsTimedOut()
        {
            // Arrange
            _timeoutsRepositoryMock.Setup(x => x.isTimeOut(allowedOrigin)).Returns(true);
            // Act
            bool result = _accessService.ShouldReplayToOrigin(allowedOrigin);
            // Assert
            Assert.IsFalse(result);
            _timeoutsRepositoryMock.Verify(x => x.setTimeOut(allowedOrigin, timeOutInMinutes), Times.Never);
        }

        [Test]
        public void VerifyToken_ReturnsTrue_WhenTokenIsValid()
        {
            // Arrange
            _cryptoServiceMock.Setup(x => x.VerifyToken(It.IsAny<Token>())).Returns(true);
            // Act
            bool result = _accessService.VerifyToken(validToken);
            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void VerifyToken_ReturnsFalse_WhenTokenIsExpierd()
        {
            // Arrange
            _cryptoServiceMock.Setup(x => x.VerifyToken(It.IsAny<Token>())).Returns(false);
            // Act
            bool result = _accessService.VerifyToken(expiredToken);
            // Assert
            Assert.IsFalse(result);
        }
    }
}
