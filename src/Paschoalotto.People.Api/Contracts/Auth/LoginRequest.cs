using System.ComponentModel.DataAnnotations;

namespace Paschoalotto.People.Api.Contracts.Auth
{
    public class LoginRequest
    {
        [Required] public string Username { get; set; } = null!;
        [Required] public string Password { get; set; } = null!;
    }
}
