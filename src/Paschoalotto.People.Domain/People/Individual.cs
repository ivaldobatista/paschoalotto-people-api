using Paschoalotto.People.Domain.People.Enums;
using Paschoalotto.People.Domain.People.ValueObjects;

namespace Paschoalotto.People.Domain.People;

public sealed class Individual : Person
{
    public string FullName { get; private set; } = null!;
    public Cpf Cpf { get; private set; } = null!;
    public DateTime BirthDate { get; private set; }
    public GenderType Gender { get; private set; }
    public string? PhotoPath { get; private set; }

    private Individual() { } // EF

    public Individual(
        string fullName, Cpf cpf, DateTime birthDate, GenderType gender,
        EmailAddress email, PhoneNumber phone, Address address)
    {
        FullName = fullName;
        Cpf = cpf;
        BirthDate = birthDate;
        Gender = gender;
        Email = email;
        Phone = phone;
        Address = address;
    }

    public void UpdatePhoto(string? path)
    {
        PhotoPath = path;
        Touch();
    }
}
