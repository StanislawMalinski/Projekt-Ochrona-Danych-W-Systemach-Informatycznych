
using projekt.Db.BankContext;
using projekt.Models.Dtos;
using projekt.Models.Requests;
using projekt.Services;
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
        var result = _bankDbContext.Accounts
            .Select(x => DecryptAccount(x))
            .FirstOrDefault(x => x.Email == email && x.IsVerified);
        return result;
    }

    public Account GetAccount(string accountNumber){
        var result = _bankDbContext.Accounts
            .Select(x => DecryptAccount(x))
            .FirstOrDefault(x => x.AccountNumber == accountNumber && x.IsVerified);
        return result;
    }

    public Account Register(RegisterRequest request){
        cleanUp();
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
        account = _bankDbContext.Accounts
            .Select(x => DecryptAccount(x))
            .FirstOrDefault(x => x.Email == request.Email);
        return account;
    }

    public Account Register(Account account){
        cleanUp();
        account.AccountNumber = GenerateAccountNumber();
        account.Salt = _cryptoService.GenerateSalt();
        account.Password = _cryptoService.HashPassword(account.Password, account.Salt);
        account.IsVerified = false;
        EncryptAccount(account);
        _bankDbContext.Accounts.Add(account);
        _bankDbContext.SaveChanges();
        return account;
    }

    public bool VerifyAccount(string email){
        try{
            var account = _bankDbContext.Accounts
                .Select(x => DecryptAccount(x))
                .FirstOrDefault(x => x.Email == email && !x.IsVerified);
            if (account == null) return false;
            account.IsVerified = true;
            EncryptAccount(account);
            _bankDbContext.SaveChanges();
            cleanUp();
            return true;
        } catch(Exception e){
            return false;
        }
    }

    public bool validUser(LoginRequest request){
        try{
            var account = _bankDbContext.Accounts
                .Select(x => DecryptAccount(x))
                .FirstOrDefault(x => x.Email == request.Email && x.IsVerified);
            if (account == null) return false;
            var salt = account.Salt;
            var password = _cryptoService.HashPassword(request.Password, salt);
            cleanUp();
            return password == account.Password;
        }catch(Exception e){
            return false;
        }
    }

    public bool CheckIfAccountExistsByEmail(string Email){
        try{
            var result = _bankDbContext.Accounts
                .Select(x => DecryptAccount(x))
                .FirstOrDefault(x => x.Email == Email && x.IsVerified);
            cleanUp();
            return result != null;
        } catch(Exception e){
            return false;
        }
    }

    public bool CheckIfAccountExistsByAccountNumber(string AccountNumber){
        var result = _bankDbContext.Accounts
            .Select(x => DecryptAccount(x))
            .FirstOrDefault(x => x.AccountNumber == AccountNumber && x.IsVerified);
        cleanUp();
        return result != null;
    }

    public bool CheckIfNotVerifiedAccountExistsByEmail(string Email){
        var result = _bankDbContext.Accounts
            .Select(x => DecryptAccount(x))
            .FirstOrDefault(x => x.Email == Email && !x.IsVerified);
        cleanUp();
        return result != null;
    }

    private string GenerateAccountNumber(){
        var last_nr = _bankDbContext.Accounts
            .Select(x => DecryptAccount(x))
            .Max(x => x.AccountNumber);
        if (last_nr == null) return "567843560";
        cleanUp();
        return "" + (int.Parse(last_nr) + 1);
    }

    public bool isTransferPossible(string accountNumber, decimal value){
        var account = _bankDbContext.Accounts
            .Select(x => DecryptAccount(x))
            .FirstOrDefault(x => x.AccountNumber == accountNumber);
        if (account == null) return false;
        cleanUp();
        return account.Balance >= value;
    }

    public bool makeTransfer(Transfer transfer){
        try {
            var account = _bankDbContext.Accounts
                .Select(x => DecryptAccount(x))
                .FirstOrDefault(x => x.AccountNumber == transfer.AccountNumber);
            account.Balance -= transfer.Value;
            var recipient = _bankDbContext.Accounts
                .Select(x => DecryptAccount(x))
                .FirstOrDefault(x => x.AccountNumber == transfer.RecipentAccountNumber);
            recipient.Balance += transfer.Value;
            EncryptAccount(account);
            EncryptAccount(recipient);
            _bankDbContext.SaveChanges();
            cleanUp();
            return true;
        } catch(Exception e){
            return false;
        }
    }

    public bool ChangePassword(string email, string password)
    {
        try{
            var account = _bankDbContext.Accounts
                .Select(x => DecryptAccount(x))
                .FirstOrDefault(x => x.Email == email && x.IsVerified);
            if (account == null) return false;
            account.Password = _cryptoService.HashPassword(password, account.Salt);
            EncryptAccount(account);
            _bankDbContext.SaveChanges();
            cleanUp();
            return true;
        } catch(Exception e){
            return false;
        }
    }

    private void cleanUp(){
        var accounts = _bankDbContext.Accounts
            .Select(x => DecryptAccount(x))
            .Where(x => !x.IsVerified)
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