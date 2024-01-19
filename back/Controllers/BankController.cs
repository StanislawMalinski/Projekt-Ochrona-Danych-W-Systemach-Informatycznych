using Microsoft.AspNetCore.Mvc;
using projekt.Models.Requests;
using projekt.Serivces;

namespace projekt.Controllers;

[ApiController]
[Route("bank")]
public class BankController : ControllerBase
{
    private readonly ILogger<BankController> _logger;
    private readonly IBankService _bankService;
    private readonly IActivityService _activityService;

    public BankController(ILogger<BankController> logger, IBankService bankService, IActivityService activityService)
    {
        _logger = logger;
        _bankService = bankService;
        _activityService = activityService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var c = HttpContext;
        Console.WriteLine(c);
        var response = _bankService.Login(request);
        return Ok(response);
    }

    [HttpPost("register")]
    public IActionResult register([FromBody] RegisterRequest request)
    {
        var c = HttpContext;
        var response = _bankService.Register(request);
        return Ok(response);
    } 

    [HttpPost("pass-change")]
    public IActionResult PassChange([FromBody] PassChangeRequest request)
    {
        var response = _bankService.ChangePassword(request);
        return Ok(response);
    }

    [HttpPost("transfer")]
    public IActionResult Transfer([FromBody] TransferRequest request)
    {
        var response = _bankService.NewTransfer(request);
        return Ok(response);
    }

    [HttpPost("account")]
    public IActionResult Account([FromBody] AccountRequest request)
    {
        var response = _bankService.GetAccount(request);
        return Ok(response);
    }
}