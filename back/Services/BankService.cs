
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

    public AccountResponse GetAccount(AccountRequest request)
    {
        var result = _accountRepository.GetAccount(request.AccountNumber);
        if(result == null) { return null; }
        return new AccountResponse{
            AccountNumber = result.AccountNumber,
            Balance = result.Balance,
            History = _transferRepository.GetHistory(request.AccountNumber)
        };
    }

    public RegisterResponse Register(RegisterRequest request)
    {
        var accountExists = _accountRepository.CheckIfAccountExists(request.Email);
        if(accountExists) { return null; }
        var result = _accountRepository.Register(request);
        return new RegisterResponse{
            AccountNumber = result.AccountNumber,
            Balance = 0,
            History = new List<Transfer>()
        };
    }

    public Account Login(LoginRequest request)
    {
        var result = _accountRepository.Login(request);

        return Account
    }

    public AccountResponse NewTransfer(TransferRequest request)
    {
        return new AccountResponse{
            AccountNumber = "",
            Balance = 0,
            History = new List<Transfer>()
        };
    }
}