using Paschoalotto.People.Domain.People.Enums;

namespace Paschoalotto.People.Api.Contracts.Responses;

public sealed class IndividualResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Cpf { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public GenderType Gender { get; set; }
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
    public string? PhotoPath { get; set; }
    public string? PhotoUrl { get; set; }
}
