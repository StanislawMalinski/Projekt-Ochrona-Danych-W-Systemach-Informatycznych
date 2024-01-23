using projekt.Models.Dtos;
using projekt.Models.Requests;
namespace projekt.Db.Repository;

public interface IAccountRepository
{
    public Account GetAccountByEmail(string email);
    public Account GetAccount(string accountNumber);
    public bool ChangePassword(string email, string password);
    public Account Register(RegisterRequest request);
    public Account Register(Account account);
    public bool VerifyAccount(string email);
    public bool validUser(LoginRequest request);
    public bool CheckIfAccountExistsByEmail(string Email);
    public bool CheckIfAccountExistsByAccountNumber(string AccountNumber);
    public bool CheckIfNotVerifiedAccountExistsByEmail(string Email);
    public bool isTransferPossible(string accountNumber, decimal value);
    public bool makeTransfer(Transfer transfer);
}