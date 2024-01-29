
using projekt.Db.BankContext;
using projekt.Models.Dtos;
using projekt.Models.Requests;
using projekt.Db.Repository.Interfaces;
using projekt.Services.Interfaces;

namespace projekt.Db.Repository;

public class AccountRepository : IAccountRepository
{
    private readonly BankDbContext _bankDbContext;
    private readonly ICryptoService _cryptoService;

    public AccountRepository(BankDbContext bankDbContext, ICryptoService cryptoService)
    {
        _bankDbContext = bankDbContext;
        _cryptoService = cryptoService;
    }

    public Account GetAccountByEmail(string email){
        email = _cryptoService.EncryptString(email);
        var result = _bankDbContext.Accounts
            .FirstOrDefault(x => x.Email == email && x.IsVerified);
        if (result == null) return new Account();
        return DecryptAccount(result);
    }

    public Account GetAccount(string accountNumber){
        accountNumber = _cryptoService.EncryptString(accountNumber);
        var result = _bankDbContext.Accounts
            .FirstOrDefault(x => x.AccountNumber == accountNumber && x.IsVerified);
        if (result == null) return new Account();
        return DecryptAccount(result);
    }

    public Account Register(RegisterRequest request){
        CleanUp();
        var salt = _cryptoService.GenerateSalt();
        var account = new Account{
            AccountNumber = GenerateAccountNumber(),
            Name = request.Name,
            Email = request.Email,
            Balance = 100.00M,
            Password = _cryptoService.HashPassword( request.Password,salt),
            Salt = salt,
            IsVerified = false
        };
        EncryptAccount(account);
        _bankDbContext.Accounts.Add(account);
        _bankDbContext.SaveChanges();
        var email = _cryptoService.EncryptString(request.Email);
        account = _bankDbContext.Accounts
            .FirstOrDefault(x => x.Email == email);
        if (account == null) return new Account();
        return DecryptAccount(account);
    }

    public Account Register(Account account){
        CleanUp();
        account.AccountNumber = GenerateAccountNumber();
        account.Salt = _cryptoService.GenerateSalt();
        account.Password = _cryptoService.HashPassword(account.Password, account.Salt);
        account.IsVerified = false;
        EncryptAccount(account);
        _bankDbContext.Accounts.Add(account);
        _bankDbContext.SaveChanges();
        return _cryptoService.DecryptAccount(account);
    }

    public bool VerifyAccount(string email){
        email = _cryptoService.EncryptString(email);
        var account = _bankDbContext.Accounts
            .FirstOrDefault(x => x.Email == email && !x.IsVerified);
        if (account == null) return false;
        account.IsVerified = true;
        EncryptAccount(account);
        _bankDbContext.SaveChanges();
        CleanUp();
        return true;
    }

    public bool validUser(LoginRequest request){
        var email = _cryptoService.EncryptString(request.Email);
        var account = _bankDbContext.Accounts
            .FirstOrDefault(x => x.Email == email && x.IsVerified);
        if (account == null) return false;
        var salt = _cryptoService.DecryptString(account.Salt);
        var password = _cryptoService.HashPassword(request.Password, salt);
        CleanUp();
        return password == account.Password;
    }

    public bool CheckIfAccountExistsByEmail(string email){
        var encryptedEmail = _cryptoService.EncryptString(email);
        var result = _bankDbContext.Accounts
            .FirstOrDefault(x => x.Email == email && x.IsVerified);
        CleanUp();
        return result != null;
    }

    public bool CheckIfAccountExistsByAccountNumber(string accountNumber){
        var encryptedAccountNumber = _cryptoService.EncryptString(accountNumber);
        var result = _bankDbContext.Accounts
            .FirstOrDefault(x => x.AccountNumber == accountNumber && x.IsVerified);
        CleanUp();
        return result != null;
    }

    public bool CheckIfNotVerifiedAccountExistsByEmail(string email)
    {
        var encryptedEmail = _cryptoService.EncryptString(email);
        var account = _bankDbContext.Accounts
            .FirstOrDefault(x => x.Email == encryptedEmail && !x.IsVerified);
        return account != null;
    }

    private string GenerateAccountNumber(){
        var last_nr = _bankDbContext.Accounts
            .Select(x => DecryptAccount(x))
            .Max(x => x.AccountNumber);
        if (last_nr == null) return "567843560";
        CleanUp();
        return "" + (int.Parse(last_nr) + 1);
    }

    public bool isTransferPossible(string accountNumber, decimal value){
        accountNumber = _cryptoService.EncryptString(accountNumber);
        var account = _bankDbContext.Accounts
            .FirstOrDefault(x => x.AccountNumber == accountNumber);
        if (account == null) return false;
        CleanUp();
        return account.Balance >= value;
    }

    public bool makeTransfer(Transfer transfer){
        var accountNumber = _cryptoService.EncryptString(transfer.AccountNumber);
        var recipientAccountNumber = _cryptoService.EncryptString(transfer.RecipentAccountNumber);
        var account = _bankDbContext.Accounts
            .FirstOrDefault(x => x.AccountNumber == accountNumber);
        var recipient = _bankDbContext.Accounts
            .FirstOrDefault(x => x.AccountNumber == recipientAccountNumber);
        if (account == null) return false;
        if (recipient == null) return false;
        account.Balance -= transfer.Value;
        recipient.Balance += transfer.Value;
        _bankDbContext.SaveChanges();
        CleanUp();
        return true;
    }

    public bool ChangePassword(string email, string password)
    {
        email = _cryptoService.EncryptString(email);
        var account = _bankDbContext.Accounts
            .FirstOrDefault(x => x.Email == email && x.IsVerified);
        if (account == null) return false;
        DecryptAccount(account);
        account.Password = _cryptoService.HashPassword(password, account.Salt);
        EncryptAccount(account);
        _bankDbContext.SaveChanges();
        CleanUp();
        return true;
    }

    private void CleanUp(){
        var accounts = _bankDbContext.Accounts
            .Where(x => !x.IsVerified)
            .Select(x => DecryptAccount(x))
            .Join(_bankDbContext.Verifications,  x => x.Email, y => y.Email, (x, y) => new {x, y})
            .Where(x => x.y.Date < DateTime.Now.AddMinutes(-5))
            .Select(x => x.x)
            .ToList();
        _bankDbContext.Accounts
            .RemoveRange(accounts);
        _bankDbContext.SaveChanges();
    }

    private Account EncryptAccount(Account account){
        return _cryptoService.EncryptAccount(account);
    }

    private Account DecryptAccount(Account account){
        return _cryptoService.DecryptAccount(account);
    }
}