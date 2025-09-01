using System.ComponentModel.DataAnnotations;

namespace Paschoalotto.People.Api.Contracts;

public sealed class CreateLegalEntityRequest
{
    [Required, MaxLength(200)]
    public string CorporateName { get; set; } = null!;

    [Required, MaxLength(200)]
    public string TradeName { get; set; } = null!;

    [Required, MaxLength(18)] // 00.000.000/0000-00
    public string Cnpj { get; set; } = null!;

    [MaxLength(40)]
    public string? StateRegistration { get; set; }

    [MaxLength(40)]
    public string? MunicipalRegistration { get; set; }

    [Required, MaxLength(200)]
    public string LegalRepresentativeName { get; set; } = null!;

    [Required, MaxLength(14)]
    public string LegalRepresentativeCpf { get; set; } = null!;

    [Required, MaxLength(255)]
    public string Email { get; set; } = null!;

    [Required, MaxLength(32)]
    public string Phone { get; set; } = null!;

    [Required]
    public AddressDto Address { get; set; } = null!;

    [MaxLength(255)]
    public string? LogoPath { get; set; }
}
