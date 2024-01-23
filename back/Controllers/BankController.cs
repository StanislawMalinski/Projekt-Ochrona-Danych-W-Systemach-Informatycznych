using Microsoft.AspNetCore.Mvc;
using projekt.Models.Requests;
using projekt.Serivces;
using projekt.Models.Enums;

using System.Text.Json;
using System.Linq;

namespace projekt.Controllers;

[ApiController]
[Route("bank")]
public class BankController : ControllerBase
{
    private readonly IBankService _bankService;
    private readonly IActivityService _activityService;

    public BankController(IBankService bankService, IActivityService activityService)
    {
        _bankService = bankService;
        _activityService = activityService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        Thread.Sleep(1000);
        var origin = Request.Headers["Origin"].ToString() ?? "unknown";
        if (!request.IsValid())
            return BadRequest("Email or password cannot be empty");
        try {
            var response = _bankService.Login(request);
            _activityService.LogActivity(ActivityType.Login, request.Email, origin, response.Success);
            return Ok(response);
        } catch (Exception e) {
            _activityService.LogActivity(ActivityType.Login, request.Email, origin,false);
            return BadRequest();
        }
    }

    [HttpPost("register")]
    public IActionResult register([FromBody] RegisterRequest request)
    {
        Thread.Sleep(1000);
        var origin = Request.Headers["Origin"].ToString() ?? "unknown";
        if (!request.IsValid())
            return BadRequest("Email or password cannot be empty");
        try { 
            var response = _bankService.Register(request);
            _activityService.LogActivity(ActivityType.Register, request.Email,origin, response.Success);
            return Ok(response);
        } catch (Exception e) {
            _activityService.LogActivity(ActivityType.Register, request.Email,origin, false);
            return BadRequest();
        }
    } 

    [HttpPost("code-submit-register")]
    public IActionResult CodeSubmitRegister([FromBody] CodeSubmitRequest request)
    {
        Thread.Sleep(1000);
        var origin = Request.Headers["Origin"].ToString() ?? "unknown";
        if (!request.IsValid())
            return BadRequest("Email or password cannot be empty");
        try {
            var response = _bankService.CodeSubmitRegister(request);
            _activityService.LogActivity(ActivityType.CodeSubmit, request.Email,origin, response.Success);
            return Ok(response);
        } catch (Exception e) {
            _activityService.LogActivity(ActivityType.CodeSubmit, request.Email,origin, false);
            return BadRequest();
        }
    }

    [HttpPost("pass-change-request-code")]
    public IActionResult PassChangeRequest([FromBody] PassChangeRequestCode request)
    {
        Thread.Sleep(1000);
        var origin = Request.Headers["Origin"].ToString() ?? "unknown";
        if (!request.IsValid())
            return BadRequest("Email cannot be empty");
        try {
            var response = _bankService.ChangePasswordCodeRequest(request);
            _activityService.LogActivity(ActivityType.ChangePasswordCodeRequest, request.Email,origin, response.Success);
            return Ok(response);
        } catch (Exception e) {
            _activityService.LogActivity(ActivityType.ChangePasswordCodeRequest, request.Email,origin, false);
            return BadRequest();
        }
    }

    [HttpPost("code-submit-pass-change")]
    public IActionResult CodeSubmit([FromBody] CodeSubmitRequest request)
    {
        Thread.Sleep(1000);
        var origin = Request.Headers["Origin"].ToString() ?? "unknown";
        if (!request.IsValid())
            return BadRequest("Email or password cannot be empty");
        try {
            var response = _bankService.CodeSubmit(request);
            _activityService.LogActivity(ActivityType.CodeSubmit, request.Email, origin,response.Success);
            return Ok(response);
        } catch (Exception e) {
            _activityService.LogActivity(ActivityType.CodeSubmit, request.Email,origin, false);
            return BadRequest();
        }
    }

    [HttpPost("pass-change")]
    public IActionResult PassChange([FromBody] PasswordChangeRequest request)
    {
        Thread.Sleep(1000);
        var origin = Request.Headers["Origin"].ToString() ?? "unknown";
        if (!request.IsValid())
            return BadRequest("Email or password cannot be empty");
        try {
            var response = _bankService.ChangePassword(request);
            _activityService.LogActivity(ActivityType.ChangePassword, request.Email,origin, response.Success);
            return Ok(response);
        } catch (Exception e) {
            _activityService.LogActivity(ActivityType.ChangePassword, request.Email,origin, false);
            return BadRequest();
        }
    }

    [HttpPost("transfer")]
    public IActionResult Transfer([FromBody] TransferRequest request)
    {
        Thread.Sleep(1000);
        var origin = Request.Headers["Origin"].ToString() ?? "unknown";
        if (!request.IsValid())
            return BadRequest("You made some errors");
        try{
            var response = _bankService.NewTransfer(request);
            _activityService.LogActivity(ActivityType.NewTransfer, request.RecipientAccountNumber, origin, response.Success);
            _activityService.LogActivity(ActivityType.NewTransfer, request.AccountNumber, origin, response.Success);
            return Ok(response);
        } catch (Exception e) {
            _activityService.LogActivity(ActivityType.NewTransfer, request.RecipientAccountNumber,origin,  false);
            _activityService.LogActivity(ActivityType.NewTransfer, request.AccountNumber, origin, false);
            return BadRequest();
        }
    }

    [HttpPost("account")]
    public IActionResult Account([FromBody] AccountRequest request)
    {
        Thread.Sleep(1000);
        var origin = Request.Headers["Origin"].ToString() ?? "unknown";
        if (!request.IsValid())
            return BadRequest("Email cannot be empty");
        try{
            var response = _bankService.GetAccount(request);
            _activityService.LogActivity(ActivityType.GetAccount, request.Email, origin, response.Success);
            return Ok(response);
        } catch (Exception e) {
            _activityService.LogActivity(ActivityType.GetAccount, request.Email, origin, false);
            return BadRequest();
        }
    }

    [HttpGet("pubkey")]
    public IActionResult GetPublicKey()
    {
        _activityService.LogActivity(ActivityType.Login, true);
        return Ok(CryptoService.GetPublicKey());
    }

    [HttpGet("activities")]
    public IActionResult GetActivities([FromQuery] string email)
    {
        Thread.Sleep(1000);
        var origin = Request.Headers["Origin"].ToString() ?? "unknown";
        if (Validator.validEmail(email) == false)
            return BadRequest("Invalid email");
        try {
            var response = _activityService.GetActivities(email);
            _activityService.LogActivity(ActivityType.GetActivities, email, origin, true);
            return Ok(response);
        } catch (Exception e) {
            _activityService.LogActivity(ActivityType.GetActivities, email, origin, false);
            return BadRequest();
        }
    }
}