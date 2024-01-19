

using projekt.Models.Requests;
using projekt.Models.Responses;

namespace projekt.Serivces;

public class BankService : IBankService
{
    public PassChangeResponse ChangePassword(PassChangeRequest request)
    {   
        var response = new PassChangeResponse
        {
            AccountNumber = "",
        };
        return response;
    }

    public AccountResponse GetAccount(AccountRequest accountNumber)
    {
        return new AccountResponse{
            AccountNumber = "",
            Balance = 0,
            Currency = ""
        };
    }

    public LoginResponse Login(LoginRequest request)
    {
        return new LoginResponse{
            AccountNumber = "",
            Balance = 0,
            Currency = ""
        };
    }

    public AccountResponse NewTransfer(TransferRequest request)
    {
        return new AccountResponse{
            AccountNumber = "",
            Balance = 0,
            Currency = ""
        };
    }
}