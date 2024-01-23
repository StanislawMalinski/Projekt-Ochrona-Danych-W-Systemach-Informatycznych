using Microsoft.AspNetCore.Mvc;
using projekt.Models.Requests;
using projekt.Serivces;
using projekt.Models.Enums;

using System.Text.Json;

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
        try {
            var response = _bankService.Login(request);
            _activityService.LogActivity(ActivityType.Login, request.Email, response.Success);
            return Ok(response);
        } catch (Exception e) {
            _activityService.LogActivity(ActivityType.Login, request.Email, false);
            return BadRequest(e.Message);
        }
    }

    [HttpPost("register")]
    public IActionResult register([FromBody] RegisterRequest request)
    {
        try { 
            var response = _bankService.Register(request);
            _activityService.LogActivity(ActivityType.Register, request.Email, response.Success);
            return Ok(response);
        } catch (Exception e) {
            _activityService.LogActivity(ActivityType.Register, request.Email, false);
            return BadRequest(e.Message);
        }
    } 

    [HttpPost("code-submit-register")]
    public IActionResult CodeSubmitRegister([FromBody] CodeSubmitRequest request)
    {
        try {
            var response = _bankService.CodeSubmitRegister(request);
            _activityService.LogActivity(ActivityType.CodeSubmit, request.Email, response.Success);
            return Ok(response);
        } catch (Exception e) {
            _activityService.LogActivity(ActivityType.CodeSubmit, request.Email, false);
            return BadRequest(e.Message);
        }
    }

    [HttpPost("pass-change-request-code")]
    public IActionResult PassChangeRequest([FromBody] PassChangeRequestCode request)
    {
        try {
            var response = _bankService.ChangePasswordCodeRequest(request);
            _activityService.LogActivity(ActivityType.ChangePasswordCodeRequest, request.Email, response.Success);
            return Ok(response);
        } catch (Exception e) {
            _activityService.LogActivity(ActivityType.ChangePasswordCodeRequest, request.Email, false);
            return BadRequest(e.Message);
        }
    }

    [HttpPost("code-submit-pass-change")]
    public IActionResult CodeSubmit([FromBody] CodeSubmitRequest request)
    {
        try {
            var response = _bankService.CodeSubmit(request);
            _activityService.LogActivity(ActivityType.CodeSubmit, request.Email, response.Success);
            return Ok(response);
        } catch (Exception e) {
            _activityService.LogActivity(ActivityType.CodeSubmit, request.Email, false);
            return BadRequest(e.Message);
        }
    }

    [HttpPost("pass-change")]
    public IActionResult PassChange([FromBody] PasswordChangeRequest request)
    {
        try {
            var response = _bankService.ChangePassword(request);
            _activityService.LogActivity(ActivityType.ChangePassword, request.Email, response.Success);
            return Ok(response);
        } catch (Exception e) {
            _activityService.LogActivity(ActivityType.ChangePassword, request.Email, false);
            return BadRequest(e.Message);
        }
    }

    [HttpPost("transfer")]
    public IActionResult Transfer([FromBody] TransferRequest request)
    {;
        try{
            var response = _bankService.NewTransfer(request);
            _activityService.LogActivity(ActivityType.NewTransfer, request.RecipientAccountNumber, response.Success);
            _activityService.LogActivity(ActivityType.NewTransfer, request.AccountNumber, response.Success);
            return Ok(response);
        } catch (Exception e) {
            _activityService.LogActivity(ActivityType.NewTransfer,  false);
            _activityService.LogActivity(ActivityType.NewTransfer, request.RecipientAccountNumber, false);
            _activityService.LogActivity(ActivityType.NewTransfer, request.AccountNumber, false);
            return BadRequest(e.Message);
        }
    }

    [HttpPost("account")]
    public IActionResult Account([FromBody] AccountRequest request)
    {
        try{
            var response = _bankService.GetAccount(request);
            _activityService.LogActivity(ActivityType.GetAccount, request.Email, response.Success);
            return Ok(response);
        } catch (Exception e) {
            _activityService.LogActivity(ActivityType.GetAccount,  false);
            return BadRequest(e.Message);
        }
    }

    [HttpGet("pubkey")]
    public IActionResult GetPublicKey()
    {
        _activityService.LogActivity(ActivityType.Login, true);
        return Ok(CryptoService.GetPublicKey());
    }

    [HttpGet("activities/{email}")]
    public IActionResult GetActivities([FromQuery] string email)
    {
        try {
            var response = _activityService.GetActivities(email);
            _activityService.LogActivity(ActivityType.GetActivities, email, true);
            return Ok(response);
        } catch (Exception e) {
            _activityService.LogActivity(ActivityType.GetActivities, email, false);
            return BadRequest(e.Message);
        }
    }
}