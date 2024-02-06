using projekt.Services;
using projekt.Services.Interfaces;

namespace tests.Services;
[TestFixture]
public class CryptoServiceTest
{
    private CryptoService _cryptoService;

    private static string plainText = "test";
    private static string password = "test123";
    private static string iv = "test123";
    private static string accountNumber = "123456789";
    private static int userId = 1;
    private static int sessionId = 1;


    [SetUp]
    public void Setup()
    {
        var inMemorySettings = new Dictionary<string, string> {
            {"Secrets:DbSecret", password},
            {"Secrets:InitVector", iv}
        };
        var _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
        _cryptoService = new CryptoService(_configuration);
    }

    [Test]
    public void HashPassword_ShouldReturnHashedPassword()
    {
        // Given
        var salt = _cryptoService.GenerateSalt();
        // When
        var result = _cryptoService.HashPassword(password, salt);
        // Then
        Assert.IsNotNull(result);
        Assert.AreNotEqual(result, password);
    }

    [Test]
    public void EncryptDbEntry_ShouldReturnEncryptedString()
    {
        // Given
        // When
        var result = _cryptoService.EncryptDbEntry(plainText);

        // Then
        Assert.IsNotNull(result);
        Assert.AreNotEqual(string.Empty, result);
        Assert.AreNotEqual(plainText, result);
    }

    [Test]
    public void DecryptDbEntry_ShouldReturnDecryptedString()
    {
        // Given
        var encryptedPhrase = _cryptoService.EncryptDbEntry(plainText);
        // When
        var result = _cryptoService.DecryptDbEntry(encryptedPhrase);
        // Then
        Assert.IsNotNull(result);
        Assert.AreEqual(plainText, result);
    }

    [Test]
    public void GenerateToken_ShouldReturnValidToken()
    {
        // Given
        DateTime expirationDate = DateTime.Now.AddHours(1);

        // When
        var result = _cryptoService.GenerateToken(userId, sessionId, expirationDate);

        // Then
        Assert.IsNotNull(result);
        Assert.AreEqual(sessionId, result.SessionId);
        Assert.AreEqual(expirationDate, result.ExpirationDate);
        Assert.IsNotNull(result.Sign);
    }

    [Test]
    public void VerifyToken_ShouldReturnTrueForValidToken()
    {
        // Given
        DateTime expirationDate = DateTime.Now.AddHours(1);
        var token = _cryptoService.GenerateToken(userId, sessionId, expirationDate);
        // When
        var result = _cryptoService.VerifyToken(token);
        // Then
        Assert.IsTrue(result);
    }

    [Test]
    public void GenerateToken_ShouldReturnDifferentSignsForDifferentInputs()
    {
        // Given
        DateTime expirationDate1 = DateTime.Now.AddHours(1);
        DateTime expirationDate2 = DateTime.Now.AddHours(1);

        // When
        var result1 = _cryptoService.GenerateToken(userId, sessionId, expirationDate1);
        var result2 = _cryptoService.GenerateToken(userId, sessionId, expirationDate2);

        // Then
        Assert.AreNotEqual(result1.Sign, result2.Sign);
    }
}