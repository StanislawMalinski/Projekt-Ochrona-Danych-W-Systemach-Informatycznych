using projekt.Db.BankContext;
using projekt.Db.Repository.Interfaces;
using projekt.Db.Repository;
using projekt.Services.Interfaces;
using projekt.Services;
using projekt.Models.Dtos;
using Microsoft.EntityFrameworkCore;

public class AccountRepositoryTest
{
    private Mock<BankDbContext> _bankDbContextMock;
    private Mock<ICryptoService> _cryptoServiceMock;
    private Mock<DbSet<Account>> _mockSet;

    private IAccountRepository _accountRepository;

    private static int id = 1;
    private static string name = "test";
    private static string accountNumber = "123456789";
    private static string email = "email@test.com";
    private static decimal balance = 0;
    private static string password = "test123";
    private static string salt = "salt123";
    private static bool isVerified = true;
    private static Account encryptedAccount = new Account
    {
        Id = id,
        Name = name,
        AccountNumber = accountNumber,
        Email = "enc" + email,
        Balance = balance,
        Password = "enc" + password,
        Salt = "enc" + salt,
        IsVerified = isVerified
    };

    private static Account account = new Account {
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
        _bankDbContextMock = new Mock<BankDbContext>();
        _cryptoServiceMock = new Mock<ICryptoService>();
        _mockSet = new Mock<DbSet<Account>>();

        _accountRepository = new AccountRepository(_bankDbContextMock.Object, _cryptoServiceMock.Object);
    }

    //[Test]
    public void shouldReturnAccountByEmail()
    {
        // Given
        var accounts = new List<Account> { encryptedAccount }.AsQueryable();
        _bankDbContextMock.Setup(x => x.Accounts).Returns(_mockSet.Object); // Change _mockSet to _mockSet.Object
        _mockSet.As<IQueryable<Account>>().Setup(x => x.Provider).Returns(accounts.Provider);
        _mockSet.As<IQueryable<Account>>().Setup(x => x.Expression).Returns(accounts.Expression);
        _mockSet.As<IQueryable<Account>>().Setup(x => x.ElementType).Returns(accounts.ElementType);
        _mockSet.As<IQueryable<Account>>().Setup(x => x.GetEnumerator()).Returns(accounts.GetEnumerator());
        _cryptoServiceMock.Setup(x => x.DecryptAccount(encryptedAccount)).Returns(account);

        // When
        var result = _accountRepository.GetAccountByEmail(email); // Change accountRepository to _accountRepository

        // Then
        Assert.AreEqual(result, account);
    }

    //[Test]
    public void GetAccountByEmail_ShouldReturnAccount_WhenEmailIsValid()
    {
        // Given
        var accounts = new List<Account> { encryptedAccount }.AsQueryable();
        _bankDbContextMock.Setup(x => x.Accounts).Returns(_mockSet.Object);
        _mockSet.As<IQueryable<Account>>().Setup(x => x.Provider).Returns(accounts.Provider);
        _mockSet.As<IQueryable<Account>>().Setup(x => x.Expression).Returns(accounts.Expression);
        _mockSet.As<IQueryable<Account>>().Setup(x => x.ElementType).Returns(accounts.ElementType);
        _mockSet.As<IQueryable<Account>>().Setup(x => x.GetEnumerator()).Returns(accounts.GetEnumerator());
        _cryptoServiceMock.Setup(x => x.DecryptAccount(encryptedAccount)).Returns(account);

        // When
        var result = _accountRepository.GetAccountByEmail(email);

        // Then
        Assert.AreEqual(result, account);
    }

    //[Test]
    public void GetAccountByEmail_ShouldReturnNull_WhenEmailIsInvalid()
    {
        // Given
        var accounts = new List<Account> { encryptedAccount }.AsQueryable();
        _bankDbContextMock.Setup(x => x.Accounts).Returns(_mockSet.Object);
        _mockSet.As<IQueryable<Account>>().Setup(x => x.Provider).Returns(accounts.Provider);
        _mockSet.As<IQueryable<Account>>().Setup(x => x.Expression).Returns(accounts.Expression);
        _mockSet.As<IQueryable<Account>>().Setup(x => x.ElementType).Returns(accounts.ElementType);
        _mockSet.As<IQueryable<Account>>().Setup(x => x.GetEnumerator()).Returns(accounts.GetEnumerator());
        _cryptoServiceMock.Setup(x => x.DecryptAccount(encryptedAccount)).Returns(account);

        // When
        var result = _accountRepository.GetAccountByEmail("invalidEmail@test.com");

        // Then
        Assert.IsNull(result);
    }
}
