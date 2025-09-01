using System.Security.Claims;

namespace Paschoalotto.People.Application.Abstractions;

public interface ITokenService
{
    string CreateToken(string subject, string role, IEnumerable<Claim>? extraClaims = null);
    int GetExpiresInSeconds(); 
}
