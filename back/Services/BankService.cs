
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
    private readonly IAccessService _accessService;
    private readonly IActivityRepository _acctivityRepository;
    private readonly ITransferRepository _transferRepository;
    private readonly IVerificationRepository _verificationRepository;
    private readonly IDebugSerivce _debug_service;
    private readonly IConfiguration _congifuration;

    public BankService(IAccountRepository accountRepository,
        IAccessService accessService, 
        IActivityRepository acctivityRepository,
        ITransferRepository transferRepository, 
        IVerificationRepository verificationRepository,
        IDebugSerivce debug_service,
        IConfiguration configuration)
    {
        _accountRepository = accountRepository;
        _acctivityRepository = acctivityRepository;
        _accessService = accessService;
        _transferRepository = transferRepository;
        _verificationRepository = verificationRepository;
        _debug_service = debug_service;
        _congifuration = configuration;
    }

    public BasicResponse Register(RegisterRequest request)
    {
        var accountExists = _accountRepository.CheckIfAccountExistsByEmail(request.Email);
        if(accountExists) return new BasicResponse { Message = "Account with this email already exists.", Success = false };
        var result = _accountRepository.Register(request);
        var verification_code = GenerateVerificationCode();
        _verificationRepository.CreateVerification(result.Id, verification_code);
        SendVerificationMessage(request.Email, verification_code);
        return new BasicResponse{Message = "Awaits for verification.",Success = true};
    }

    public BasicResponse CodeSubmitRegister(CodeSubmitRequest request){
        var accountExists = _accountRepository.CheckIfAccountExistsByEmail(request.Email, false);
        var errorResponse = new BasicResponse {Message = "Sorry, your validation code has expiered.", Success = false};
        if(!accountExists) return errorResponse; 
        var result = _accountRepository.GetAccountByEmail(request.Email, false);
        var verificationIsValid = _verificationRepository.CheckIfVerificationIsValid(result.Id, request.Code);
        if(!verificationIsValid) return errorResponse; 
        _accountRepository.VerifyAccount(request.Email);
        _verificationRepository.DeleteVerification(result.Id);
        return new BasicResponse {Message = "Account has been verified. Please try to login now...", Success = true};
    }

    public AccountResponse GetAccount(AccountRequest request)
    {
        var errorResponse = new AccountResponse("Error while getting account.");
        var userId = _accessService.GetUserId(request.Token);
        Console.WriteLine(userId);
        if (userId == -1) return errorResponse;
        var result = _accountRepository.GetAccountByUserId(userId);
        Console.WriteLine(result);
        if(result == null) return errorResponse;
        if (!result.IsVerified) return errorResponse;
        return new AccountResponse{
            AccountNumber = result.AccountNumber,
            Balance = result.Balance,
            History = _transferRepository.GetHistory(result.AccountNumber),
            Message = "Account found.",
            Success = true,
            Token = request.Token
        };
    }
    public AccountResponse Login(LoginRequest request)
    {
        var ValidUser = _accountRepository.ValidUser(request);
        if(!ValidUser)  return new AccountResponse("Invalid email or password.");
        var result = _accountRepository.GetAccountByEmail(request.Email);
        if (result == null) return new AccountResponse ("Invalid email or password.");
        if (!result.IsVerified) return new AccountResponse("Account is not verified.");
        return new AccountResponse{
            AccountNumber = result.AccountNumber,
            Balance = result.Balance,
            History = _transferRepository.GetHistory(result.AccountNumber),
            Message = "Login successful.",
            Success = true,
            Token = _accessService.GetToken(result.Id)
        };
    }

   public ReleventOriginsResponse GetRelevantOrigins(Token token)
    {
        var errorResponse = new ReleventOriginsResponse("Error while getting relevant origins.");
        var userId = _accessService.GetUserId(token);
        if (userId == -1) return errorResponse;
        var result = _accountRepository.GetAccountByUserId(userId);
        if(result == null) return errorResponse;
        if (!result.IsVerified) return errorResponse;
        var origins = _acctivityRepository.GetRelevantOrigins(result.Email);
        return new ReleventOriginsResponse(){
            Message = "Origins found.",
            Success = true,
            Origins = origins
        };
    }

    public AccountResponse NewTransfer(TransferRequest request)
    {
        var errorResponse = new AccountResponse("Transfer cannot be made.");
        if (request.AccountNumber == request.RecipientAccountNumber) return errorResponse;
        var recipentAccountExists = _accountRepository.CheckIfAccountExistsByAccountNumber(request.RecipientAccountNumber);
        var accountExists = _accountRepository.CheckIfAccountExistsByAccountNumber(request.AccountNumber);
        if (!accountExists || !recipentAccountExists) return errorResponse;
        var account = _accountRepository.GetAccount(request.AccountNumber);
        var recipient = _accountRepository.GetAccount(request.RecipientAccountNumber);
        if (!account.IsVerified || !recipient.IsVerified) return errorResponse;
        if (!_accountRepository.IsTransferPossible(request.AccountNumber, request.Value)) return errorResponse;
        var transfer = new Transfer{
            AccountNumber = request.AccountNumber,
            RecipentAccountNumber = recipient.AccountNumber,
            Recipent = request.Recipient,
            Sender = account.Name,
            Value = request.Value,
            Title = request.Title,
            Date = DateTime.Now
        };
        _accountRepository.MakeTransfer(transfer);
        _transferRepository.NewTransfer(transfer);

        account = _accountRepository.GetAccount(request.AccountNumber);
        return new AccountResponse{
            AccountNumber = account.AccountNumber,
            Balance = account.Balance,
            History = _transferRepository.GetHistory(account.AccountNumber),
            Message = "Transfer was successful.",
            Success = true,
            Token = request.Token
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
        var lenCode = _congifuration.GetValue<int>("ClassConfig:BankService:VerificationCodeLength");
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
            var account = _accountRepository.GetAccountByEmail(request.Email);
            _verificationRepository.CreateVerification(account.Id, verification_code);
            SendPasswordMessageChange(request.Email, verification_code);
        }   
        return new BasicResponse { Message = "The email with verification code was sent this email address", Success = true};
    }

    public BasicResponse CodeSubmit(CodeSubmitRequest request)
    {
        var accountExists = _accountRepository.CheckIfAccountExistsByEmail(request.Email);
        if(!accountExists) return new BasicResponse {Message = "Sorry, your validation code has expiered or was invalid.",Success = false}; 
        var result = _accountRepository.GetAccountByEmail(request.Email);
        var verificationIsValid = _verificationRepository.CheckIfVerificationIsValid(result.Id, request.Code);
        if(!verificationIsValid) return new BasicResponse {Message = "Sorry, your validation code has expiered or was invalid.",Success = false}; 
        return new BasicResponse { Message = "Verification has been succesful.", Success = true};
    }

    public BasicResponse ChangePassword(PasswordChangeRequest request)
    {
        var accountExists = _accountRepository.CheckIfAccountExistsByEmail(request.Email);
        if(!accountExists) return new BasicResponse {Message = "Sorry, your validation code has expiered or was invalid.",Success = false}; 
        var result = _accountRepository.GetAccountByEmail(request.Email);
        var verificationIsValid = _verificationRepository.CheckIfVerificationIsValid(result.Id, request.Code);
        if(!verificationIsValid) return new BasicResponse {Message = "Sorry, your validation code has expiered or was invalid.",Success = false}; 
        _accountRepository.ChangePassword(request.Email, request.Password);
        _verificationRepository.DeleteVerification(result.Id);
        return new BasicResponse {Message = "Password has been changed.",Success = true};
    }

 
}