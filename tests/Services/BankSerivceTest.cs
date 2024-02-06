
using projekt.Db.Repository;
using projekt.Db.Repository.Interfaces;
using projekt.Models.Requests;
using projekt.Models.Responses;
using projekt.Models.Dtos;
using projekt.Services.Interfaces;
using projekt.Services;

namespace Tests.Services
{
    [TestFixture]
    public class BankServiceTest
    {
        private Mock<IAccountRepository> _accountRepositoryMock;
        private Mock<IAccessService> _accessServiceMock;
        private Mock<IActivityRepository> _activityRepositoryMock;
        private Mock<ITransferRepository> _transferRepositoryMock;
        private Mock<IVerificationRepository> _verificationRepositoryMock;
        private Mock<IDebugSerivce> _debugServiceMock;
        private IConfiguration _configuration;

        private BankService _bankService;

        private static int id = 1;
        private static string name = "test";
        private static string accountNumber = "123456789";
        private static string email = "test@emil.com";
        private static decimal balance = 0;
        private static string password = "test123";
        private static string salt = "test123";
        private static bool isVerified = true;
        private static string sign = "test";


        private LoginRequest loginRequest = new LoginRequest
        {
            Email = email,
            Password = password
        };
        private Account account = new Account
        {
            Id = id,
            Name = name,
            AccountNumber = accountNumber,
            Email = email,
            Balance = balance,
            Password = password,
            Salt = salt,
            IsVerified = isVerified
        };

        [SetUp]
        public void Setup()
        {
            _transferRepositoryMock = new Mock<ITransferRepository>();
            _accessServiceMock = new Mock<IAccessService>();
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _activityRepositoryMock = new Mock<IActivityRepository>();
            _verificationRepositoryMock = new Mock<IVerificationRepository>();
            _debugServiceMock = new Mock<IDebugSerivce>();

            var inMemorySettings = new Dictionary<string, string> {
                {"key", "value"},
            };
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            _bankService = new BankService(
                _accountRepositoryMock.Object, 
                _accessServiceMock.Object,
                _activityRepositoryMock.Object,
                _transferRepositoryMock.Object,
                _verificationRepositoryMock.Object,
                _debugServiceMock.Object, 
                _configuration);
        }

        [Test]
        public void LoginTest()
        {
            //Given
            _accountRepositoryMock.Setup(x => x.ValidUser(loginRequest)).Returns(true);
            _accountRepositoryMock.Setup(x => x.GetAccountByEmail(loginRequest.Email, true)).Returns(account);
            _transferRepositoryMock.Setup(x => x.GetHistory(account.AccountNumber)).Returns(new List<Transfer>());
            //When
            var result = _bankService.Login(loginRequest);
            //Then
            Assert.AreEqual(result.Message, "Verification code has been sent to your email address.");
        }
    }
}
