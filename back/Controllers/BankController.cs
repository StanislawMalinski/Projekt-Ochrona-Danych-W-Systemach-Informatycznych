using Microsoft.AspNetCore.Mvc;
using projekt.Models.Requests;
using projekt.Services.Interfaces;
using projekt.Models.Enums;
using projekt.Models.Dtos;
using projekt.Models.Responses;

namespace projekt.Controllers;

[ApiController]
[Route("bank")]
public class BankController : ControllerBase
{
    private readonly IBankService _bankService;
    private readonly IActivityService _activityService;
    private readonly IAccessService _accessService;
    private readonly IConfiguration _configuration;

    public BankController(IBankService bankService, IActivityService activityService, IAccessService accessService, IConfiguration configuration)
    {
        _bankService = bankService;
        _activityService = activityService;
        _accessService = accessService;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        delay();
        var origin = getOrigin();
        var response = ValidateRequest(request, origin);
        response = response.Success ? _bankService.Login(request) : response;
        _activityService.LogActivity(ActivityType.Login, request.Email, origin, response.Success);
        return getResponse(response);
    }

    [HttpPost("login-code-submit")]
    public IActionResult LoginCodeSubmit([FromBody] CodeSubmitRequest request)
    {
        delay();
        var origin = getOrigin();
        var response = ValidateRequest(request, origin);
        response = response.Success ? _bankService.LoginCodeSubmit(request) : response;
        _activityService.LogActivity(ActivityType.CodeSubmit, request.Email, origin, response.Success);
        return getResponse(response);
    }

    [HttpPost("register")]
    public IActionResult register([FromBody] RegisterRequest request)
    {
        delay();
        var origin = getOrigin();
        var response = ValidateRequest(request, origin);
        response = response.Success ? _bankService.Register(request) : response;
        _activityService.LogActivity(ActivityType.Register, request.Email, origin, response.Success);
        return getResponse(response);
    } 

    [HttpPost("code-submit-register")]
    public IActionResult CodeSubmitRegister([FromBody] CodeSubmitRequest request)
    {
        delay();
        var origin = getOrigin();
        var response = ValidateRequest(request, origin);
        response = response.Success ? _bankService.CodeSubmitRegister(request) : response;
        _activityService.LogActivity(ActivityType.CodeSubmit, request.Email, origin, response.Success);
        return getResponse(response);
    }

    [HttpPost("pass-change-request-code")]
    public IActionResult PassChangeRequest([FromBody] PassChangeRequestCode request)
    {
        delay();
        var origin = getOrigin();
        var response = ValidateRequest(request, origin);
        response = response.Success ? _bankService.ChangePasswordCodeRequest(request) : response;
        _activityService.LogActivity(ActivityType.ChangePasswordCodeRequest, request.Email, origin, response.Success);
        return getResponse(response);
    }

    [HttpPost("code-submit-pass-change")]
    public IActionResult CodeSubmit([FromBody] CodeSubmitRequest request)
    {
        delay();
        var origin = getOrigin();
        var response = ValidateRequest(request, origin);
        response = response.Success ? _bankService.CodeSubmit(request) : response;
        _activityService.LogActivity(ActivityType.CodeSubmit, request.Email, origin, response.Success);
        return getResponse(response);
    }

    [HttpPost("pass-change")]
    public IActionResult PassChange([FromBody] PasswordChangeRequest request)
    {
        delay();
        var origin = getOrigin();
        var response = ValidateRequest(request, origin);
        response = response.Success ? _bankService.ChangePassword(request) : response;
        _activityService.LogActivity(ActivityType.ChangePassword, request.Email, origin, response.Success);
        return getResponse(response);
    }

    [HttpPost("logout")]
    public IActionResult Logout([FromBody] LogoutRequest request)
    {
        delay();
        var origin = getOrigin();
        var response = ValidateRequest(request, origin, request.Token);
        response = response.Success ? _bankService.Logout(request) : response;
        _activityService.LogActivity(ActivityType.Logout, origin, response.Success, origin: true);
        return getResponse(response);
    }

    [HttpPost("transfer")]
    public IActionResult Transfer([FromBody] TransferRequest request)
    {
        delay();
        var origin = getOrigin();
        var response = ValidateRequest(request, origin, request.Token);
        response = response.Success ? _bankService.NewTransfer(request) : response;
        _activityService.LogActivity(ActivityType.NewTransfer, request.AccountNumber, origin, response.Success);
        return getResponse(response);
    }

    [HttpPost("account")]
    public IActionResult Account([FromBody] AccountRequest request)
    {
        delay();
        var origin = getOrigin();
        var response = ValidateRequest(request, origin, request.Token);
        response = response.Success ? _bankService.GetAccount(request) : response;
        _activityService.LogActivity(ActivityType.GetAccount, origin, response.Success, origin: true);
        return getResponse(response);
    }

    private void delay(){
        var delay = _configuration.GetSection("ClassConfig:Delay").Get<int>();
        if(delay > 0) Thread.Sleep(delay);
    }

    private string getOrigin(){
        return Request.Headers.Origin.ToString() ?? "unknown";
    }

    private BasicResponse ValidateRequest(BasicRequest request, string origin, Token token){
        var response = ValidateRequest(request, origin);
        var tokenIsValid = _accessService.VerifyToken(token);
        response.Success = response.Success && tokenIsValid;
        if (!tokenIsValid) response.Message = "Authentication failed";
        return response;
    }

    private BasicResponse ValidateRequest(BasicRequest request, string origin){
        var validatorMessage = request.IsValid();
        return new BasicResponse(){
            Success = _accessService.ShouldReplayToOrigin(origin) && string.IsNullOrEmpty(validatorMessage),
            Message = validatorMessage
        };
    }

    private IActionResult getResponse(BasicResponse response)
    {
        if (response.Success) return Ok(response);
        var basicResponse = new BasicResponse { Message = response.Message, Success = false };
        return BadRequest(basicResponse);
    }
}