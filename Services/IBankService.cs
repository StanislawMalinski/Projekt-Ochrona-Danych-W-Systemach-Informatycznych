using projekt.Models.Requests;
using projekt.Models.Responses;

namespace projekt.Serivces;

public interface IBankService
{
    LoginResponse Login(LoginRequest request);
    AccountResponse GetAccount(AccountRequest accountNumber);
    AccountResponse NewTransfer(TransferRequest request);
    PassChangeResponse ChangePassword(PassChangeRequest request);
}