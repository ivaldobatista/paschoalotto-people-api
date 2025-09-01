using System.ComponentModel.DataAnnotations;

namespace Paschoalotto.People.Api.Contracts;

public sealed class AddressDto
{
    [Required, MaxLength(120)]
    public string Street { get; set; } = null!;

    [Required, MaxLength(15)]
    public string Number { get; set; } = null!;

    [MaxLength(60)]
    public string? Complement { get; set; }

    [Required, MaxLength(80)]
    public string District { get; set; } = null!;

    [Required, MaxLength(80)]
    public string City { get; set; } = null!;

    [Required, MaxLength(40)]
    public string State { get; set; } = null!;

    [Required, MaxLength(20)]
    public string ZipCode { get; set; } = null!;

    [Required, MaxLength(60)]
    public string Country { get; set; } = null!;
}
