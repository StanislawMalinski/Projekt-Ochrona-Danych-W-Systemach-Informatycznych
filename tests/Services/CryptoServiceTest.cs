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
    public void Encrypt_ShouldReturnEncryptedString()
    {
        // Given
        // When
        var result = _cryptoService.Encrypt(plainText);

        // Then
        Assert.IsNotNull(result);
        Assert.AreNotEqual(result, plainText);
    }

    [Test]
    public void GenerateToken_ShouldReturnValidToken()
    {
        // Given
        // When
        var result = _cryptoService.GenerateToken(accountNumber);

        // Then
        Assert.IsNotNull(result);
        Assert.AreEqual(result.AccountNumber, accountNumber);
        Assert.IsTrue(_cryptoService.verifyToken(result));
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
}