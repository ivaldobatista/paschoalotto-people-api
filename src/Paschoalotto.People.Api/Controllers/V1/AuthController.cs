using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Paschoalotto.People.Api.Contracts.Auth;
using Paschoalotto.People.Application.Abstractions;

namespace Paschoalotto.People.Api.Controllers.V1;

[ApiController]
[Route("api/v1/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IConfiguration _cfg;
    private readonly ITokenService _tokens;
    private readonly ILogger<AuthController> _logger;
    private readonly IAuditLogger _audit;

    public AuthController(IConfiguration cfg, 
        ITokenService tokens, 
        IAuditLogger audit,
        ILogger<AuthController> logger)
    {
        _cfg = cfg;
        _tokens = tokens;
        _logger = logger;
        _audit = audit;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] LoginRequest req)
    {
        var u = _cfg["Auth:Username"];
        var p = _cfg["Auth:Password"];
        var r = _cfg["Auth:Role"] ?? "admin";

        if (!string.Equals(req.Username, u, StringComparison.OrdinalIgnoreCase) ||
            !string.Equals(req.Password, p, StringComparison.Ordinal))
        {
            _logger.LogWarning("Tentativa de login inválida para {User}", req.Username);
            return Unauthorized(new { error = "Credenciais inválidas." });
        }

        var token = _tokens.CreateToken(subject: u!, role: r);
        _audit.Log("Login", "User", req.Username, null);
        return Ok(new LoginResponse
        {
            AccessToken = token,
            ExpiresIn = _tokens.GetExpiresInSeconds()
        });
    }
}
