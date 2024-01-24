
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
    private readonly IDebugSerivce _debug_service;

    public BankService(IAccountRepository accountRepository, ITransferRepository transferRepository, IVerificationRepository verificationRepository, IDebugSerivce debug_service)
    {
        _accountRepository = accountRepository;
        _transferRepository = transferRepository;
        _verificationRepository = verificationRepository;
        _debug_service = debug_service;
    }

    public SimpleResponse Register(RegisterRequest request)
    {
        var accountExists = _accountRepository.CheckIfAccountExistsByEmail(request.Email);
        if(accountExists) { return new SimpleResponse { Message = "Account with this email already exists.", Success = false };}
        var result = _accountRepository.Register(request);
        var verification_code = GenerateVerificationCode();
        _verificationRepository.CreateVerification(request.Email, verification_code);
        SendVerificationMessage(request.Email, verification_code);
        return new SimpleResponse{
            Message = "Awaits for verification.",
            Success = true
        };
    }

    public SimpleResponse CodeSubmitRegister(CodeSubmitRequest request){
        var accountExists = _accountRepository.CheckIfNotVerifiedAccountExistsByEmail(request.Email);
        if(!accountExists) { 
            return new SimpleResponse {
                Message = "Sorry, your validation code has expiered.",
                Success = false
            }; 
        }
        var verificationIsValid = _verificationRepository.CheckIfVerificationIsValid(request.Email, request.Code);
        if(!verificationIsValid) { 
            return new SimpleResponse {
                Message = "Sorry, your validation code has expiered.",
                Success = false
            }; 
        }
        _accountRepository.VerifyAccount(request.Email);
        _verificationRepository.DeleteVerification(request.Email);
        return new SimpleResponse {
            Message = "Account has been verified. Please try to login now...",
            Success = true
        };
    }

    public AccountResponse GetAccount(AccountRequest request)
    {
        var result = _accountRepository.GetAccountByEmail(request.Email);
        if(result == null) 
            return new AccountResponse {
                AccountNumber = "",
                Balance = 0,
                History = [],
                Message = "Account with this number does not exist.",
                Success = false
            };
        if (!result.IsVerified) 
            return new AccountResponse {
                AccountNumber = "",
                Balance = 0,
                History = [],
                Message = "Account is not verified.",
                Success = false
            };
        return new AccountResponse{
            AccountNumber = result.AccountNumber,
            Balance = result.Balance,
            History = _transferRepository.GetHistory(result.AccountNumber),
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
        if (!result.IsVerified)  return new AccountResponse { AccountNumber = "", Balance = 0, History = new List<Transfer>(), Message = "Account is not verified.", Success = false };
        return new AccountResponse{
            AccountNumber = result.AccountNumber,
            Balance = result.Balance,
            History = _transferRepository.GetHistory(result.AccountNumber),
            Message = "Login successful.",
            Success = true,
            Token = CryptoService.GenerateToken(result.AccountNumber)
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
        var recipient = _accountRepository.GetAccount(request.RecipientAccountNumber);
        if (!account.IsVerified) 
            return new AccountResponse { AccountNumber = "", Balance = 0, History = new List<Transfer>(), Message ="Sender account does not exist.", Success = false};
        if (!recipient.IsVerified)
            return new AccountResponse { AccountNumber = "", Balance = 0, History = new List<Transfer>(), Message ="Recipient account does not exist.", Success = false};
        if (account.AccountNumber == recipient.AccountNumber) 
            return new AccountResponse { AccountNumber = "", Balance = 0, History = new List<Transfer>(), Message ="You cannot transfer money to yourself.", Success = false};
        var transfer = new Transfer{
            AccountNumber = request.AccountNumber,
            RecipentAccountNumber = recipient.AccountNumber,
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
            Success = true,
            Token = CryptoService.GenerateToken(account.AccountNumber)
        };
    }

    private void SendVerificationMessage(string email, string verification_code)
    {
        var message = $"Dear customer. Here is your verification code: {verification_code}";
        _debug_service.LogMessage(email, message);
    }

    private void SendPasswordMessageChange(string email, string verificationCode){
        var message = $"Dear customer. There is pending password change on your account. Here is your verification code: {verificationCode}";
        _debug_service.LogMessage(email, message);
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

    public SimpleResponse ChangePasswordCodeRequest(PassChangeRequestCode request)
    {
        var accountExists = _accountRepository.CheckIfAccountExistsByEmail(request.Email);
        if(accountExists){
            var verification_code = GenerateVerificationCode();
            _verificationRepository.CreateVerification(request.Email, verification_code);
            SendPasswordMessageChange(request.Email, verification_code);
        }   
        return new SimpleResponse { Message = "The email with verification code was sent this email address", Success = true};
    }

    public SimpleResponse CodeSubmit(CodeSubmitRequest request)
    {
        var accountExists = _accountRepository.CheckIfAccountExistsByEmail(request.Email);
        if(!accountExists) { 
            return new SimpleResponse {
                Message = "Sorry, your validation code has expiered.",
                Success = false
            }; 
        }
        var verificationIsValid = _verificationRepository.CheckIfVerificationIsValid(request.Email, request.Code);
        if(!verificationIsValid) { 
            return new SimpleResponse {
                Message = "Sorry, your validation code has expiered.",
                Success = false
            }; 
        }
        return new SimpleResponse {
            Message = "Verification has been succesful.",
            Success = true
        };
    }

    public SimpleResponse ChangePassword(PasswordChangeRequest request)
    {
        var accountExists = _accountRepository.CheckIfAccountExistsByEmail(request.Email);
        if(!accountExists) { 
            return new SimpleResponse {
                Message = "Sorry, your validation code has expiered.",
                Success = false
            }; 
        }
        var verificationIsValid = _verificationRepository.CheckIfVerificationIsValid(request.Email, request.Code);
        if(!verificationIsValid) { 
            return new SimpleResponse {
                Message = "Sorry, your validation code has expiered.",
                Success = false
            }; 
        }
        _accountRepository.ChangePassword(request.Email, request.Password);
        _verificationRepository.DeleteVerification(request.Email);
        return new SimpleResponse {
            Message = "Password has been changed.",
            Success = true
        };
    }
}

/*
 public PassChangeResponse ChangePassword(PassChangeRequestCode request)
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
    */