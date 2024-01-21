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
            _activityService.LogActivity(ActivityType.Login, response.Success);
            return Ok(response);
        } catch (Exception e) {
            _activityService.LogActivity(ActivityType.Login, false);
            return BadRequest(e.Message);
        }
    }

    [HttpPost("register")]
    public IActionResult register([FromBody] RegisterRequest request)
    {
        try { 
            var response = _bankService.Register(request);
            _activityService.LogActivity(ActivityType.Register, response.Success);
            return Ok(response);
        } catch (Exception e) {
            _activityService.LogActivity(ActivityType.Register, false);
            return BadRequest(e.Message);
        }
    } 

    [HttpPost("pass-change")]
    public IActionResult PassChange([FromBody] PassChangeRequest request)
    {
        try {
            var response = _bankService.ChangePassword(request);
            _activityService.LogActivity(ActivityType.ChangePassword, response.Success);
            return Ok(response);
        } catch (Exception e) {
            _activityService.LogActivity(ActivityType.ChangePassword, false);
            return BadRequest(e.Message);
        }
    }

    [HttpPost("transfer")]
    public IActionResult Transfer([FromBody] TransferRequest request)
    {;
        try{
            var response = _bankService.NewTransfer(request);
            _activityService.LogActivity(ActivityType.NewTransfer,  response.Success);
            return Ok(response);
        } catch (Exception e) {
            _activityService.LogActivity(ActivityType.NewTransfer,  false);
            return BadRequest(e.Message);
        }
    }

    [HttpPost("account")]
    public IActionResult Account([FromBody] AccountRequest request)
    {
        try{
            var response = _bankService.GetAccount(request);
            _activityService.LogActivity(ActivityType.GetAccount,  response.Success);
            return Ok(response);
        } catch (Exception e) {
            _activityService.LogActivity(ActivityType.GetAccount,  false);
            return BadRequest(e.Message);
        }
    }

    [HttpGet("pubKey")]
    public IActionResult GetPublicKey()
    {
        _activityService.LogActivity(ActivityType.Login, true);
        return Ok(CryptoService.GetPublicKey());
    }
}