using Microsoft.AspNetCore.Mvc;
using projekt.Serivces;

public class DebugController : ControllerBase
{
    private readonly IDebugSerivce _debugService;
    public DebugController(IDebugSerivce debugService)
    {
        _debugService = debugService;
    }

    //[HttpPost("seed-db")]
    public IActionResult SeedDb()
    {
        _debugService.SeedDatabase();
        return Ok();
    }
}