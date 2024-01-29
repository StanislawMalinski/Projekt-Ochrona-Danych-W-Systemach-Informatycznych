using projekt.Services;
using projekt.Services.Interfaces;

namespace tests.Services;
public class CryptoServiceTest
{
    private CryptoService _cryptoService;

    private static string plainText = "test";
    private static string password = "test123";
    private static string iv = "test123";
    private static string accountNumber = "123456789";


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
        int sessionId = 123;
        DateTime expirationDate = DateTime.Now.AddHours(1);

        // When
        var result = _cryptoService.GenerateToken(sessionId, expirationDate);

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
        int sessionId = 123;
        DateTime expirationDate = DateTime.Now.AddHours(1);
        var token = _cryptoService.GenerateToken(sessionId, expirationDate);
        // When
        var result = _cryptoService.VerifyToken(token);
        // Then
        Assert.IsTrue(result);
    }

    [Test]
    public void GenerateToken_ShouldReturnDifferentSignsForDifferentInputs()
    {
        // Given
        int sessionId1 = 123;
        DateTime expirationDate1 = DateTime.Now.AddHours(1);

        int sessionId2 = 123;
        DateTime expirationDate2 = DateTime.Now.AddHours(1);

        // When
        var result1 = _cryptoService.GenerateToken(sessionId1, expirationDate1);
        var result2 = _cryptoService.GenerateToken(sessionId2, expirationDate2);

        // Then
        Assert.AreNotEqual(result1.Sign, result2.Sign);
    }
}