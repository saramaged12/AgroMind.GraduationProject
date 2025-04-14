using Microsoft.AspNetCore.Mvc;

[Route("api/test")]
[ApiController]
public class TestController : ControllerBase
{
    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok("App is running");
    }
}
