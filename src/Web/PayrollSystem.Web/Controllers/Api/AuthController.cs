using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayrollSystem.Web.Auth;

namespace PayrollSystem.Web.Controllers.Api;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly ITokenService _tokens;

    public AuthController(IConfiguration config, ITokenService tokens)
    {
        _config = config;
        _tokens = tokens;
    }

    public sealed record LoginRequest(string UserName, string Password);

    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var adminUser = _config["Admin:User"] ?? "admin";
        var adminPass = _config["Admin:Password"] ?? "admin123";

        if (!string.Equals(request.UserName, adminUser, StringComparison.Ordinal) ||
            !string.Equals(request.Password, adminPass, StringComparison.Ordinal))
        {
            return Unauthorized(new { message = "Invalid credentials." });
        }

        var token = _tokens.CreateToken(request.UserName, roles: ["Admin"]);
        return Ok(new { access_token = token, token_type = "Bearer" });
    }
}

