using projekt.Models.Dtos;
using projekt.Models.Requests;
namespace projekt.Db.Repository;

public interface IAccountRepository
{
    public Account GetAccount(string accountNumber);
    public Account ChangePassword(PassChangeRequest request);
    public Account Register(RegisterRequest request);
    public bool validUser(LoginRequest request);
    public bool CheckIfAccountExists(string Email);
}