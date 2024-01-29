
using projekt.Db.Repository;
using projekt.Db.Repository.Interfaces;
using projekt.Models.Requests;
using projekt.Models.Responses;
using projekt.Models.Dtos;
using projekt.Services.Interfaces;

namespace projekt.Services;

public class BankService : IBankService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransferRepository _transferRepository;
    private readonly IVerificationRepository _verificationRepository;
    private readonly IDebugSerivce _debug_service;
    private readonly ICryptoService _cryptoSerivce;
    private readonly IConfiguration _congifuration;

    public BankService(IAccountRepository accountRepository, 
        ITransferRepository transferRepository, 
        IVerificationRepository verificationRepository, 
        IDebugSerivce debug_service,
        ICryptoService cryptoSerivce,
        IConfiguration configuration)
    {
        _accountRepository = accountRepository;
        _transferRepository = transferRepository;
        _verificationRepository = verificationRepository;
        _debug_service = debug_service;
        _cryptoSerivce = cryptoSerivce;
        _congifuration = configuration;
    }

    public BasicResponse Register(RegisterRequest request)
    {
        var accountExists = _accountRepository.CheckIfAccountExistsByEmail(request.Email);
        if(accountExists) return new BasicResponse { Message = "Account with this email already exists.", Success = false };
        var result = _accountRepository.Register(request);
        var verification_code = GenerateVerificationCode();
        _verificationRepository.CreateVerification(request.Email, verification_code);
        SendVerificationMessage(request.Email, verification_code);
        return new BasicResponse{Message = "Awaits for verification.",Success = true};
    }

    public BasicResponse CodeSubmitRegister(CodeSubmitRequest request){
        var accountExists = _accountRepository.CheckIfNotVerifiedAccountExistsByEmail(request.Email);
        if(!accountExists) 
            return new BasicResponse {Message = "Sorry, your validation code has expiered.", Success = false}; 
        var verificationIsValid = _verificationRepository.CheckIfVerificationIsValid(request.Email, request.Code);
        if(!verificationIsValid)
            return new BasicResponse {Message = "Sorry, your validation code has expiered.", Success = false}; 
        _accountRepository.VerifyAccount(request.Email);
        _verificationRepository.DeleteVerification(request.Email);
        return new BasicResponse {Message = "Account has been verified. Please try to login now...", Success = true};
    }

    public AccountResponse GetAccount(AccountRequest request)
    {
        var result = _accountRepository.GetAccountByEmail(request.Email);
        if(result == null) 
            return new AccountResponse("Account with this number does not exist.");
        if (!result.IsVerified) 
            return new AccountResponse("Account is not verified.");
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
        if(!validUser)  return new AccountResponse("Invalid email or password.");
        var result = _accountRepository.GetAccountByEmail(request.Email);
        if (result == null)  return new AccountResponse ("Account with this email does not exist.");
        if (!result.IsVerified)  return new AccountResponse("Account is not verified.");
        return new AccountResponse{
            AccountNumber = result.AccountNumber,
            Balance = result.Balance,
            History = _transferRepository.GetHistory(result.AccountNumber),
            Message = "Login successful.",
            Success = true,
            Token = _cryptoSerivce.GenerateToken(result.AccountNumber)
        };
    }

    public AccountResponse NewTransfer(TransferRequest request)
    {
        var accountExists = _accountRepository.CheckIfAccountExistsByAccountNumber(request.RecipientAccountNumber);
        if (!accountExists) 
            return new AccountResponse("Recipient account does not exist.");
        accountExists = _accountRepository.CheckIfAccountExistsByAccountNumber(request.AccountNumber);
        if (!accountExists) 
            return new AccountResponse ("Sender account does not exist.");
        if (request.Value <= 0) 
            return new AccountResponse ("Your resources are insufficient.");
        if (!_accountRepository.isTransferPossible(request.AccountNumber, request.Value)) 
            return new AccountResponse ("Your resources are insufficient.");
        var account = _accountRepository.GetAccount(request.AccountNumber);
        var recipient = _accountRepository.GetAccount(request.RecipientAccountNumber);
        if (!account.IsVerified) 
            return new AccountResponse ("Sender account does not exist.");
        if (!recipient.IsVerified)
            return new AccountResponse ("Recipient account does not exist.");
        if (account.AccountNumber == recipient.AccountNumber) 
            return new AccountResponse ("You cannot transfer money to yourself.");
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
            Token = _cryptoSerivce.GenerateToken(account.AccountNumber)
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
        var lenCode = _congifuration.GetValue<int>("BankService:VerificationCodeLength");
        for (int i = 0; i < lenCode; i++)
        {
            verificationCode += random.Next(0, 9);
        }
        return verificationCode;
    }

    public BasicResponse ChangePasswordCodeRequest(PassChangeRequestCode request)
    {
        var accountExists = _accountRepository.CheckIfAccountExistsByEmail(request.Email);
        if(accountExists){
            var verification_code = GenerateVerificationCode();
            _verificationRepository.CreateVerification(request.Email, verification_code);
            SendPasswordMessageChange(request.Email, verification_code);
        }   
        return new BasicResponse { Message = "The email with verification code was sent this email address", Success = true};
    }

    public BasicResponse CodeSubmit(CodeSubmitRequest request)
    {
        var accountExists = _accountRepository.CheckIfAccountExistsByEmail(request.Email);
        if(!accountExists) return new BasicResponse {Message = "Sorry, your validation code has expiered.",Success = false}; 
        var verificationIsValid = _verificationRepository.CheckIfVerificationIsValid(request.Email, request.Code);
        if(!verificationIsValid) return new BasicResponse {Message = "Sorry, your validation code has expiered.",Success = false}; 
        return new BasicResponse { Message = "Verification has been succesful.", Success = true};
    }

    public BasicResponse ChangePassword(PasswordChangeRequest request)
    {
        var accountExists = _accountRepository.CheckIfAccountExistsByEmail(request.Email);
        if(!accountExists) return new BasicResponse {Message = "Sorry, your validation code has expiered.",Success = false}; 
        var verificationIsValid = _verificationRepository.CheckIfVerificationIsValid(request.Email, request.Code);
        if(!verificationIsValid) return new BasicResponse {Message = "Sorry, your validation code has expiered.",Success = false}; 
        _accountRepository.ChangePassword(request.Email, request.Password);
        _verificationRepository.DeleteVerification(request.Email);
        return new BasicResponse {Message = "Password has been changed.",Success = true};
    }

    public bool ValidateToken(Token token, string EmailOrAccountNumber)
    {
        var validToken = _cryptoSerivce.verifyToken(token);
        validToken = validToken && (token.Expiration > DateTime.Now);
        Account account = new Account();
        if (Validator.validEmail(EmailOrAccountNumber))
            account = _accountRepository.GetAccountByEmail(EmailOrAccountNumber);
        else if (Validator.validNumber(EmailOrAccountNumber))
            account = _accountRepository.GetAccount(EmailOrAccountNumber);
        else
            return false;
        if (account == null) return false;
        validToken = validToken && account.AccountNumber == token.AccountNumber;
        return validToken;
    }
}