using projekt.Models.Requests;
using projekt.Models.Responses;

namespace projekt.Serivces;

public interface IBankService
{
    AccountResponse Login(LoginRequest request);
    AccountResponse Register(RegisterRequest request);
    SimpleResponse CodeSubmitRegister(CodeSubmitRequest request);
    AccountResponse GetAccount(AccountRequest accountNumber);
    AccountResponse NewTransfer(TransferRequest request);    
    SimpleResponse ChangePasswordCodeRequest(PassChangeRequestCode request);
    SimpleResponse CodeSubmit(CodeSubmitRequest request);
    SimpleResponse ChangePassword(PasswordChangeRequest request);
}