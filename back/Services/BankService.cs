
using projekt.Db.Repository;
using projekt.Models.Requests;
using projekt.Models.Responses;
using projekt.Models.Dtos;

namespace projekt.Serivces;

public class BankService : IBankService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransferRepository _transferRepository;
    private readonly IVerificationRepository _verificationRepository;

    public BankService(IAccountRepository accountRepository, ITransferRepository transferRepository, IVerificationRepository verificationRepository)
    {
        _accountRepository = accountRepository;
        _transferRepository = transferRepository;
        _verificationRepository = verificationRepository;
    }

    public AccountResponse  Register(RegisterRequest request)
    {
        var accountExists = _accountRepository.CheckIfAccountExistsByEmail(request.Email);
        if(accountExists) { return new AccountResponse { AccountNumber = "", Balance = 0, History = new List<Transfer>(), Message = "Account with this email already exists.", Success = false };}
        var result = _accountRepository.Register(request);
        return new AccountResponse{
            AccountNumber = result.AccountNumber,
            Balance = result.Balance,
            History = [],
            Message = "Registration successful.",
            Success = true
        };
    }
 public PassChangeResponse ChangePassword(PassChangeRequest request)
    {   
        var accountExists = _accountRepository.CheckIfAccountExistsByEmail(request.Email);
        if(!accountExists) { 
            return new PassChangeResponse {
                Message = "Account with this email does not exist.",
                Success = false
            }; 
        }
        var verification_code = GenerateVerificationCode();
        _verificationRepository.CreateVerification(request.Email, verification_code);
        SendPasswordMessageChange(request.Email, verification_code);
        return new PassChangeResponse {
            Message = "Verification code was sent to your email.",
            Success = true
        };
    }

    public AccountResponse GetAccount(AccountRequest request)
    {
        var result = _accountRepository.GetAccount(request.AccountNumber);
        if(result == null) 
            return new AccountResponse {
                AccountNumber = "",
                Balance = 0,
                History = [],
                Message = "Account with this number does not exist.",
                Success = false
            };
        return new AccountResponse{
            AccountNumber = result.AccountNumber,
            Balance = result.Balance,
            History = _transferRepository.GetHistory(request.AccountNumber),
            Message = "Account found.",
            Success = true
        };
    }
    public AccountResponse Login(LoginRequest request)
    {
        var validUser = _accountRepository.validUser(request);
        if(!validUser)  return new AccountResponse { AccountNumber = "", Balance = 0, History = new List<Transfer>(), Message = "Invalid email or password.", Success = false };
        var result = _accountRepository.GetAccountByEmail(request.Email);
        if (result == null)  return new AccountResponse { AccountNumber = "", Balance = 0, History = new List<Transfer>(), Message = "Account with this email does not exist.", Success = false };
        return new AccountResponse{
            AccountNumber = result.AccountNumber,
            Balance = result.Balance,
            History = _transferRepository.GetHistory(result.AccountNumber),
            Message = "Login successful.",
            Success = true
        };
    }

    public AccountResponse NewTransfer(TransferRequest request)
    {
        var accountExists = _accountRepository.CheckIfAccountExistsByAccountNumber(request.RecipientAccountNumber);
        if (!accountExists) 
            return new AccountResponse { AccountNumber = "", Balance = 0, History = new List<Transfer>(), Message = "Recipient account does not exist.", Success = false };
        accountExists = _accountRepository.CheckIfAccountExistsByAccountNumber(request.AccountNumber);
        if (!accountExists) 
            return new AccountResponse { AccountNumber = "", Balance = 0, History = new List<Transfer>(), Message ="Sender account does not exist.", Success = false};
        if (request.Value <= 0) 
            return new AccountResponse { AccountNumber = "", Balance = 0, History = new List<Transfer>(), Message ="Your resources are insufficient.", Success = false};
        if (!_accountRepository.isTransferPossible(request.AccountNumber, request.Value)) 
            return new AccountResponse { AccountNumber = "", Balance = 0, History = new List<Transfer>(), Message ="Your resources are insufficient.", Success = false};
        
        var account = _accountRepository.GetAccount(request.AccountNumber);

        var transfer = new Transfer{
            AccountNumber = request.AccountNumber,
            RecipentAccountNumber = request.RecipientAccountNumber,
            Recipent = request.Recipient,
            Sender = account.Name,
            Value = request.Value,
            Title = request.Title,
            Date = DateTime.Now
        };
        _accountRepository.makeTransfer(transfer);
        _transferRepository.NewTransfer(transfer);

        account = _accountRepository.GetAccount(request.AccountNumber);
        return new AccountResponse{
            AccountNumber = account.AccountNumber,
            Balance = account.Balance,
            History = _transferRepository.GetHistory(account.AccountNumber),
            Message = "Transfer was successful.",
            Success = true
        };
    }

    private void SendPasswordMessageChange(string email, string verificationCode){
        var message = "Dear customer. There is pending password change on your account.";
        Console.WriteLine("Send To" + email + "\n" 
                        + message + "\n"
                        + "Verification code: " + verificationCode);
    }
    private string GenerateVerificationCode()
    {
        Random random = new Random();
        string verificationCode = "";
        for (int i = 0; i < 6; i++)
        {
            verificationCode += random.Next(0, 9);
        }
        return verificationCode;
    }

}