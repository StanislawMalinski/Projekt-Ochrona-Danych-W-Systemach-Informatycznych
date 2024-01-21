
using projekt.Db.BankContext;
using projekt.Models.Dtos;
using projekt.Models.Requests;
using projekt.Models.Responses;
using projekt.Serivces;

namespace projekt.Db.Repository;

public class AccountRepository : IAccountRepository
{
    private readonly BankDbContext _bankDbContext;

    public AccountRepository(BankDbContext bankDbContext)
    {
        _bankDbContext = bankDbContext;
    }

    public Account GetAccountByEmail(string email){
        var result = _bankDbContext.Accounts.FirstOrDefault(x => x.Email == email);
        return result;
    }

    public Account GetAccount(string accountNumber){
        var result = _bankDbContext.Accounts.FirstOrDefault(x => x.AccountNumber == accountNumber);
        return result;
    }

    public Account ChangePassword(PassChangeRequest request){
        var account = _bankDbContext.Accounts.FirstOrDefault(x => x.Email == request.Email);
        return account;
    }

    public Account Register(RegisterRequest request){
        var salt = CryptoService.GenerateSalt();
        _bankDbContext.Accounts.Add(new Account{
            AccountNumber = GenerateAccountNumber(),
            Name = request.Name,
            Email = request.Email,
            Balance = 100.00M,
            Password = CryptoService.HashPassword( request.Password,salt),
            Salt = salt
        });
        _bankDbContext.SaveChanges();
        var account = _bankDbContext.Accounts.FirstOrDefault(x => x.Email == request.Email);
        return account;
    }

    public bool validUser(LoginRequest request){
        try{
            var account = _bankDbContext.Accounts.FirstOrDefault(x => x.Email == request.Email);
            if (account == null) return false;
            var salt = account.Salt;
            var password = CryptoService.HashPassword(request.Password, salt);
            return password == account.Password;
        }catch(Exception e){
            Console.WriteLine(e);
            return false;
        }
    }

    public bool CheckIfAccountExistsByEmail(string Email){
        try{
            var result = _bankDbContext.Accounts.FirstOrDefault(x => x.Email == Email);
            return result != null;
        } catch(Exception e){
            Console.WriteLine(e);
            return false;
        }
    }

    public bool CheckIfAccountExistsByAccountNumber(string AccountNumber){
        var result = _bankDbContext.Accounts.FirstOrDefault(x => x.AccountNumber == AccountNumber);
        return result != null;
    }

    private string GenerateAccountNumber(){
        var last_nr = _bankDbContext.Accounts.Max(x => x.AccountNumber);
        if (last_nr == null) return "567843560";
        return "" + (int.Parse(last_nr) + 1);
    }

    public bool isTransferPossible(string accountNumber, decimal value){
        var account = _bankDbContext.Accounts.FirstOrDefault(x => x.AccountNumber == accountNumber);
        if (account == null) return false;
        return account.Balance >= value;
    }

    public void makeTransfer(Transfer transfer){
        var account = _bankDbContext.Accounts.FirstOrDefault(x => x.AccountNumber == transfer.AccountNumber);
        account.Balance -= transfer.Value;
        var recipient = _bankDbContext.Accounts.FirstOrDefault(x => x.AccountNumber == transfer.RecipentAccountNumber);
        recipient.Balance += transfer.Value;
        _bankDbContext.SaveChanges();
    }
}