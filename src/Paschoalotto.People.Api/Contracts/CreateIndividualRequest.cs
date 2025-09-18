using Paschoalotto.People.Domain.People.Enums;
using System.ComponentModel.DataAnnotations;

namespace Paschoalotto.People.Api.Contracts;

public sealed class CreateIndividualRequest
{
    [Required, MaxLength(200)]
    public string FullName { get; set; } = null!;

    [Required, MaxLength(14)] 
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

}
