using projekt.Models.Dtos;
using projekt.Models.Requests;
using projekt.Models.Responses;

namespace projekt.Services.Interfaces;

public interface IBankService
{
    BasicResponse ChangePassword(PasswordChangeRequest request);
    BasicResponse ChangePasswordCodeRequest(PassChangeRequestCode request);
    BasicResponse CodeSubmit(CodeSubmitRequest request);
    BasicResponse CodeSubmitRegister(CodeSubmitRequest request);
    AccountResponse GetAccount(AccountRequest accountNumber);
    ReleventOriginsResponse GetRelevantOrigins(Token token);
    AccountResponse Login(LoginRequest request);
    AccountResponse NewTransfer(TransferRequest request);
    BasicResponse Register(RegisterRequest request);
}