using projekt.Models.Dtos;
using projekt.Models.Requests;
namespace projekt.Db.Repository;

public interface IAccountRepository
{
    Account GetAccount(string accountNumber);
    Account ChangePassword(PassChangeRequest request);
    Account Register(RegisterRequest request);
    Account Login(LoginRequest request);
}