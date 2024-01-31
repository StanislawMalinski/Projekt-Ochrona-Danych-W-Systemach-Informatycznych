using projekt.Controllers;
using projekt.Models.Enums;
using projekt.Models.Requests;
using projekt.Models.Responses;
using projekt.Services;
using projekt.Services.Interfaces;

using Moq;
using NUnit.Framework;

[TestFixture]
public class BankControllerTests
{
    private Mock<IBankService> _bankServiceMock;
    private Mock<IActivityService> _activityServiceMock;
    private Mock<IAccessService> _accessServiceMock;
    private IConfiguration _configuration;
    private BankController _bankController;

    [SetUp]
    public void Setup()
    {
        _bankServiceMock = new Mock<IBankService>();
        _activityServiceMock = new Mock<IActivityService>();
        _accessServiceMock = new Mock<IAccessService>();
        var inMemorySettings = new Dictionary<string, string> {
            {"key", "value"},
        };
        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        _bankController = new BankController(
            _bankServiceMock.Object,
            _activityServiceMock.Object,
            _accessServiceMock.Object,
            _configuration
        );
    }

    [Test]
    public void Test_LoginRequest_Success()
    {
        // Arrange
        var loginRequest = new LoginRequest { Email = "test@example.com", Password = "password" };
        var origin = "http://example.com";
        var response = new AccountResponse { Success = true };
        var request = requestMock.Object;
        var origin = request.Headers.ContainsKey("Origin") ? request.Headers["Origin"].ToString() : null;
        var requestMock = new Mock<HttpRequest>();
        var headersMock = new Mock<IHeaderDictionary>();
        var originHeaderValue = "http://example.com";
        
        _bankServiceMock.Setup(x => x.Login(loginRequest)).Returns(response);
        headersMock.Setup(x => x.ContainsKey("Origin")).Returns(true);
        headersMock.Setup(x => x["Origin"]).Returns(originHeaderValue);
        requestMock.Setup(x => x.Headers).Returns(headersMock.Object);
    
        // Act
        var result = _bankController.Login(loginRequest);

        // Assert
        Console.WriteLine(result);
        _bankServiceMock.Verify(x => x.Login(loginRequest), Times.Once);
        _activityServiceMock.Verify(x => x.LogActivity(ActivityType.Login, loginRequest.Email, origin, response.Success), Times.Once);
    }
}