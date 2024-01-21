using projekt.Models.Dtos;
using projekt.Models.Requests;
namespace projekt.Db.Repository;

public interface IAccountRepository
{
    public Account GetAccountByEmail(string email);
    public Account GetAccount(string accountNumber);
    public Account ChangePassword(PassChangeRequest request);
    public Account Register(RegisterRequest request);
    public bool validUser(LoginRequest request);
    public bool CheckIfAccountExistsByEmail(string Email);
    public bool CheckIfAccountExistsByAccountNumber(string AccountNumber);
    public bool isTransferPossible(string accountNumber, decimal value);
    public void makeTransfer(Transfer transfer);
}