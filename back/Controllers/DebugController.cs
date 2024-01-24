using Microsoft.AspNetCore.Mvc;
using projekt.Serivces;
using projekt.Models.Dtos;
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


    [HttpGet("token")]
    public IActionResult GetToken([FromQuery] string accountNumber)
    {
        return Ok(CryptoService.GenerateToken(accountNumber));
    }

    [HttpPost("vtoken")]
    public IActionResult VerifyToken([FromBody] Token token)
    {
        return Ok(CryptoService.verifyToken(token));
    }
}