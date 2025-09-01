using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Paschoalotto.People.Application.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Paschoalotto.People.Infrastructure.Security;

public sealed class JwtTokenService : ITokenService
{
    private readonly string _issuer;
    private readonly string _audience;
    private readonly string _key;
    private readonly int _expiresMinutes;

    public JwtTokenService(IConfiguration cfg)
    {
        _issuer = cfg["Jwt:Issuer"] ?? "Paschoalotto.People.Api";
        _audience = cfg["Jwt:Audience"] ?? "Paschoalotto.People.Clients";
        _key = cfg["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key not configured");
        _expiresMinutes = int.TryParse(cfg["Jwt:ExpiresMinutes"], out var e) ? e : 60;
    }

    public string CreateToken(string subject, string role, IEnumerable<Claim>? extraClaims = null)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, subject),
            new("role", role),
            new(ClaimTypes.Role, role),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N"))
        };
        if (extraClaims != null) claims.AddRange(extraClaims);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(_expiresMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public int GetExpiresInSeconds() => _expiresMinutes * 60;
}
