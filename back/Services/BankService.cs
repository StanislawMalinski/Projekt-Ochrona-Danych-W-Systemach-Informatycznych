
using projekt.Db.Repository;
using projekt.Models.Requests;
using projekt.Models.Responses;
using projekt.Models.Dtos;

namespace projekt.Serivces;

public class BankService : IBankService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransferRepository _transferRepository;

    public BankService(IAccountRepository accountRepository, ITransferRepository transferRepository)
    {
        _accountRepository = accountRepository;
        _transferRepository = transferRepository;
    }
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
            Currency = "",
            History = new List<Transfer>()
        };
    }

    public RegisterResponse Register(RegisterRequest request)
    {
        var result = _accountRepository.Register(request);
        var response = new RegisterResponse
        {
            AccountNumber = result.AccountNumber,
        };
        return response;
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
            Currency = "",
            History = new List<Transfer>()
        };
    }
}