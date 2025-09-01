namespace Paschoalotto.People.Api.Contracts.Auth
{
    public class LoginResponse
    {
        public string AccessToken { get; set; } = null!;
        public string TokenType { get; set; } = "Bearer";
        public int ExpiresIn { get; set; }
    }
}
