
using projekt.Db.BankContext;
using projekt.Models.Dtos;
using projekt.Models.Requests;

namespace projekt.Db.Repository;

public class AccountRepository : IAccountRepository
{
    private readonly BankDbContext _bankDbContext;

    public AccountRepository(BankDbContext bankDbContext)
    {
        _bankDbContext = bankDbContext;
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
        _bankDbContext.Accounts.Add(new Account{
            AccountNumber = GenerateAccountNumber(),
            Email = request.Email,
            Balance = 100.00M,
            Password = request.Password
        });
        _bankDbContext.SaveChanges();
        var account = _bankDbContext.Accounts.FirstOrDefault(x => x.Email == request.Email);
        return account;
    }

    public bool validUser(LoginRequest request){
        try{
            var result = _bankDbContext.Accounts.FirstOrDefault(x => x.Email == request.Email && x.Password == request.Password);
            return result != null;
        }catch(Exception e){
            Console.WriteLine(e); //TODO łagnie obsłużyć
            return false;
        }
    }

    public bool CheckIfAccountExists(string Email){
        try{
            var result = _bankDbContext.Accounts.FirstOrDefault(x => x.Email == Email);
            return result != null;
        } catch(Exception e){
            Console.WriteLine(e); //TODO łagnie obsłużyć
            return false;
        }
    }

    private string GenerateAccountNumber(){
        var last_nr = _bankDbContext.Accounts.Max(x => x.AccountNumber);
        return "" + int.Parse(last_nr) + 1;
    }
}