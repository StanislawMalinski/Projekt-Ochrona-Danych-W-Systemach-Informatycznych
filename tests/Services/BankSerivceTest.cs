
using projekt.Services.Interfaces;
using projekt.Services;

namespace Tests.Services
{
    public class BankServiceTest
    {
        private Mock<ITransferRepository> _transferRepositoryMock;
        private Mock<IAccountRepository> _accountRepositoryMock;
        private Mock<IVerificationRepository> _verificationRepositoryMock;
        private Mock<IDebugService> _debugServiceMock;
        private Mock<IConfiguration> _configurationMock;

        private Mock<BankService> _bankServiceMock;

        [SetUp]
        public void Setup()
        {
            _transferRepositoryMock = new Mock<ITransferRepository>();
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _verificationRepositoryMock = new Mock<IVerificationRepository>();
            _debugServiceMock = new Mock<IDebugService>();

            var inMemorySettings = new Dictionary<string, string> {
                {"BankService:VerificationCodeLength", 6},
            };

            _configurationMock = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
            _bankServiceMock = new Mock<BankService>(_accountRepositoryMock.Object, _transferRepositoryMock.Object, _verificationRepositoryMock.Object, _debugServiceMock.Object, _configurationMock.Object);
        }

       [Test]
        public void LoginTest()
        {
               
        }
    }
}
