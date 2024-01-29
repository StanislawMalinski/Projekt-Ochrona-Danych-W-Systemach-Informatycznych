using Microsoft.AspNetCore.Mvc;
using projekt.Services.Interfaces;
using projekt.Services;
using projekt.Models.Dtos;
using projekt.Models.Enums;

public class DebugController : ControllerBase
{
    private readonly IDebugSerivce _debugService;
    private readonly ICryptoService _cryptoService;

    private readonly IActivityService _activityService;

    public DebugController(IDebugSerivce debugService, 
        ICryptoService cryptoService,
        IActivityService activityService)
    {
        _debugService = debugService;
        _cryptoService = cryptoService;
        _activityService = activityService;
    }

    [HttpPost("seed-db")]
    public IActionResult SeedDb()
    {
        _debugService.SeedDatabase();
        return Ok();
    }

    [HttpPost("clean-db")]
    public IActionResult CleanDb()
    {
        _debugService.CleanDatabase();
        return Ok();
    }

    [HttpGet("token")]
    public IActionResult GetToken([FromQuery] int usedId)
    {
        DateTime date = DateTime.Now.AddDays(1);
        return Ok(_cryptoService.GenerateToken(usedId, date));
    }

    [HttpPost("vtoken")]
    public IActionResult VerifyToken([FromBody] Token token)
    {
        return Ok(_cryptoService.VerifyToken(token));
    }

    [HttpGet("activities")]
    public IActionResult GetActivities([FromQuery] string email)
    {
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