using System.ComponentModel.DataAnnotations;
using Paschoalotto.People.Domain.People.Enums; 

namespace Paschoalotto.People.Api.Contracts;

public sealed class CreateIndividualRequest
{
    [Required, MaxLength(200)]
    public string FullName { get; set; } = null!;

    [Required, MaxLength(14)] // 000.000.000-00
    public string Cpf { get; set; } = null!;

    [Required]
    public DateTime BirthDate { get; set; }

    [Required]
    public GenderType Gender { get; set; }

    [Required, MaxLength(255)]
    public string Email { get; set; } = null!;

    [Required, MaxLength(32)]
    public string Phone { get; set; } = null!;

    [Required]
    public AddressDto Address { get; set; } = null!;

    [MaxLength(255)]
    public string? PhotoPath { get; set; }
}
