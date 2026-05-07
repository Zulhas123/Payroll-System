using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PayrollSystem.Web.Controllers.Api;

[ApiController]
[Route("api/secure")]
public sealed class SecureController : ControllerBase
{
    [Authorize]
    [HttpGet("ping")]
    public IActionResult Ping() => Ok(new { message = "pong", user = User.Identity?.Name });
}

