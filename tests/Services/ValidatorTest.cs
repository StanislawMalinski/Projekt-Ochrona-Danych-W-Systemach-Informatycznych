using NUnit.Framework;
using projekt.Services;
using Microsoft.Extensions.Configuration;

namespace tests.Services
{
    public class ValidatorTest
    {
        private IConfiguration _configuration;

        [SetUp]
        public void Setup()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"BankService:VerificationCodeLength", "6"}
            };
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            Validator validator = new Validator(_configuration);
        }

        [Test]
        public void ValidEmail_ShouldReturnTrueForValidEmail()
        {
            Assert.IsTrue(Validator.validEmail("test@example.com"));
        }

        [Test]
        public void ValidEmail_ShouldReturnFalseForInvalidEmail()
        {
            Assert.IsFalse(Validator.validEmail("testexample.com"));
        }

        [Test]
        public void ValidCode_ShouldReturnTrueForValidCode()
        {
            Assert.IsTrue(Validator.validCode("123456"));
        }

        [Test]
        public void ValidCode_ShouldReturnFalseForInvalidCode()
        {
            Assert.IsFalse(Validator.validCode("12345a"));
        }

        [Test]
        public void ValidCode_ShouldReturnFalseForInvalidCodeLength()
        {
            Assert.IsFalse(Validator.validCode("1234567"));
        }

        [Test]
        public void ValidName_ShouldReturnTrueForValidName()
        {
            Assert.IsTrue(Validator.validName("John Doe"));
        }

        [Test]
        public void ValidName_ShouldReturnFalseForInvalidName()
        {
            Assert.IsFalse(Validator.validName("John123"));
        }

        [Test]
        public void ValidNumber_ShouldReturnTrueForValidNumber()
        {
            Assert.IsTrue(Validator.validNumber("1234567890"));
        }

        [Test]
        public void ValidNumber_ShouldReturnFalseForInvalidNumber()
        {
            Assert.IsFalse(Validator.validNumber("123456789a"));
        }

        [Test]
        public void ValidText_ShouldReturnTrueForValidText()
        {
            Assert.IsTrue(Validator.validText("Hello, World!"));
        }

        [Test]
        public void ValidText_ShouldReturnForForTextWithNumbers()
        {
            Assert.IsTrue(Validator.validText("Hello, World!123"));
        }
    }
}