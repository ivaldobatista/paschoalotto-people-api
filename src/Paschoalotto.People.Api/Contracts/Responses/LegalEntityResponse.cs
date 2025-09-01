namespace Paschoalotto.People.Api.Contracts.Responses;

public sealed class LegalEntityResponse
{
    public Guid Id { get; set; }
    public string CorporateName { get; set; } = null!;
    public string TradeName { get; set; } = null!;
    public string Cnpj { get; set; } = null!;
    public string? StateRegistration { get; set; }
    public string? MunicipalRegistration { get; set; }
    public string LegalRepresentativeName { get; set; } = null!;
    public string LegalRepresentativeCpf { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string Number { get; set; } = null!;
    public string? Complement { get; set; }
    public string District { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string ZipCode { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string? LogoPath { get; set; }
    public string? LogoUrl { get; set; }
}
