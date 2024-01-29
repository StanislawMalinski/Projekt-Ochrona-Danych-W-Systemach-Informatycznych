using projekt.Models.Dtos;
using projekt.Models.Requests;

namespace projekt.Db.Repository.Interfaces;

public interface IAccountRepository
{
    public bool ChangePassword(string email, string password);
    public bool CheckIfAccountExistsByAccountNumber(string AccountNumber);
    public bool CheckIfAccountExistsByEmail(string Email);
    public bool CheckIfNotVerifiedAccountExistsByEmail(string Email);
    public Account GetAccount(string accountNumber);
    public Account GetAccountByEmail(string email);
    public Account GetAccountByUserId(int userId);
    public bool IsTransferPossible(string accountNumber, decimal value);
    public bool MakeTransfer(Transfer transfer);
    public Account Register(Account account);
    public Account Register(RegisterRequest request);
    public bool ValidUser(LoginRequest request);
    public bool VerifyAccount(string email);
}