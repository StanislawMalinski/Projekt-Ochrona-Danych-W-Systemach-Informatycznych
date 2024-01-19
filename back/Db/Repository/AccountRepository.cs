
using projekt.Db.BankContext;
using projekt.Models.Dtos;
using projekt.Models.Requests;

namespace projekt.Db.Repository;

public class AccountRepository : IAccountRepository{
    private readonly BankDbContext _bankDbContext;

    public AccountRepository(BankDbContext bankDbContext)
    {
        _bankDbContext = bankDbContext;
    }

    public Account GetAccount(string accountNumber){
        throw new NotImplementedException();
    }

    public Account ChangePassword(PassChangeRequest request){
        throw new NotImplementedException();
    }

    public Account Register(RegisterRequest request){
        _bankDbContext.Accounts.Add(new Account{
            AccountNumber = request.AccountNumber, //TODO auto generate
            Balance = 100.00M,
            Password = request.Password
        });
        _bankDbContext.SaveChanges();
        var account = _bankDbContext.Accounts.FirstOrDefault(x => x.AccountNumber == request.AccountNumber);
        return account;
    }

    public Account Login(LoginRequest request){
        throw new NotImplementedException();
    }
}