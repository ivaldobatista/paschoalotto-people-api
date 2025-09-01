using Paschoalotto.People.Domain.Common;

namespace Paschoalotto.People.Domain.People.ValueObjects;

public sealed class Address : ValueObject
{
    public string Street { get; }
    public string Number { get; }
    public string? Complement { get; }
    public string District { get; }
    public string City { get; }
    public string State { get; }
    public string ZipCode { get; }
    public string Country { get; }

    private Address() { Street = Number = District = City = State = ZipCode = Country = string.Empty; } // EF

    public Address(
        string street, string number, string? complement, string district,
        string city, string state, string zipCode, string country)
    {
        Street = street ?? throw new ArgumentNullException(nameof(street));
        Number = number ?? throw new ArgumentNullException(nameof(number));
        Complement = complement;
        District = district ?? throw new ArgumentNullException(nameof(district));
        City = city ?? throw new ArgumentNullException(nameof(city));
        State = state ?? throw new ArgumentNullException(nameof(state));
        ZipCode = zipCode ?? throw new ArgumentNullException(nameof(zipCode));
        Country = country ?? throw new ArgumentNullException(nameof(country));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Street; yield return Number; yield return Complement;
        yield return District; yield return City; yield return State;
        yield return ZipCode; yield return Country;
    }
}
