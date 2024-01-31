using projekt.Models.Dtos;
using projekt.Models.Requests;
using projekt.Models.Responses;

namespace projekt.Services.Interfaces;

public interface IBankService
{
    AccountResponse Login(LoginRequest request);
    BasicResponse Logout(LogoutRequest request);
    BasicResponse Register(RegisterRequest request);
    BasicResponse CodeSubmitRegister(CodeSubmitRequest request);
    AccountResponse GetAccount(AccountRequest accountNumber);
    AccountResponse NewTransfer(TransferRequest request);    
    BasicResponse ChangePasswordCodeRequest(PassChangeRequestCode request);
    BasicResponse CodeSubmit(CodeSubmitRequest request);
    BasicResponse ChangePassword(PasswordChangeRequest request);
}