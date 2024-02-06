
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

    public Account GetAccountByEmail(string email, bool verified = true){
        var result = _bankDbContext.Accounts.FirstOrDefault(x => x.Email == email && x.IsVerified == verified);
        return result;
    }

    public Account GetAccount(string accountNumber){
        var result = _bankDbContext.Accounts.FirstOrDefault(x => x.AccountNumber == accountNumber && x.IsVerified);
        return result;
    }

    public Account Register(RegisterRequest request){
        cleanUp();
        var salt = _cryptoService.GenerateSalt();
        _bankDbContext.Accounts.Add(new Account{
            AccountNumber = GenerateAccountNumber(),
            Name = request.Name,
            Email = request.Email,
            Balance = 100.00M,
            Password = _cryptoService.HashPassword( request.Password,salt),
            Salt = salt,
            IsVerified = false
        });
        _bankDbContext.SaveChanges();
        var account = _bankDbContext.Accounts.FirstOrDefault(x => x.Email == request.Email);
        return account;
    }

    public Account Register(Account account){
        cleanUp();
        account.AccountNumber = GenerateAccountNumber();
        account.Salt = _cryptoService.GenerateSalt();
        account.Password = _cryptoService.HashPassword(account.Password, account.Salt);
        _bankDbContext.Accounts.Add(account);
        _bankDbContext.SaveChanges();
        return account;
    }

    public bool VerifyAccount(string email){
try{
        var account = _bankDbContext.Accounts.FirstOrDefault(x => x.Email == email && !x.IsVerified);
        if (account == null) return false;
        account.IsVerified = true;
        _bankDbContext.SaveChanges();
        cleanUp();
        return true;
} catch(Exception e){
            return false;
        }
    }

    public bool ValidUser(LoginRequest request){
try{
        var account = _bankDbContext.Accounts.FirstOrDefault(x => x.Email == request.Email && x.IsVerified);
        if (account == null) return false;
        var salt = account.Salt;
        var password = _cryptoService.HashPassword(request.Password, salt);
        cleanUp();
        return password == account.Password;
}catch(Exception e){
            return false;
        }
    }

    public bool CheckIfAccountExistsByEmail(string Email, bool verified = true){
try{
        var result = _bankDbContext.Accounts.FirstOrDefault(x => x.Email == Email && x.IsVerified == verified);
        cleanUp();
        return result != null;
} catch(Exception e){
            return false;
        }
    }

    public bool CheckIfAccountExistsByAccountNumber(string AccountNumber, bool verified = true){
        var result = _bankDbContext.Accounts.FirstOrDefault(x => x.AccountNumber == AccountNumber && x.IsVerified == verified);
        cleanUp();
        return result != null;
    }

    private string GenerateAccountNumber(){
        var last_nr = _bankDbContext.Accounts.Max(x => x.AccountNumber);
        if (last_nr == null) return "567843560";
        cleanUp();
        return "" + (int.Parse(last_nr) + 1);
    }

    public bool IsTransferPossible(string accountNumber, decimal value){
        var account = _bankDbContext.Accounts.FirstOrDefault(x => x.AccountNumber == accountNumber);
        if (account == null) return false;
        cleanUp();
        return account.Balance >= value;
    }

    public bool MakeTransfer(Transfer transfer){
try {
        var account = _bankDbContext.Accounts.FirstOrDefault(x => x.AccountNumber == transfer.AccountNumber);
        account.Balance -= transfer.Value;
        var recipient = _bankDbContext.Accounts.FirstOrDefault(x => x.AccountNumber == transfer.RecipentAccountNumber);
        recipient.Balance += transfer.Value;
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
            var account = _bankDbContext.Accounts.FirstOrDefault(x => x.Email == email && x.IsVerified);
            if (account == null) return false;
            account.Password = _cryptoService.HashPassword(password, account.Salt);
            _bankDbContext.SaveChanges();
            cleanUp();
            return true;
        } catch(Exception e){
            return false;
        }
    }

    private void cleanUp(){
        var accounts = _bankDbContext.Accounts
            .Where(x => !x.IsVerified)
            .Join(_bankDbContext.Verifications, 
                  x => x.Id, 
                  y => y.UserId, 
                  (x, y) => new {x, y})
            .Where(x => x.y.Date < DateTime.Now.AddMinutes(-5))
            .Select(x => x.x)
            .ToList();
        _bankDbContext.Accounts.RemoveRange(accounts);
        _bankDbContext.SaveChanges();
    }

    public Account GetAccountByUserId(int userId)
    {
        var result = _bankDbContext.Accounts.FirstOrDefault(x => x.Id == userId);
        return result;
    }
}