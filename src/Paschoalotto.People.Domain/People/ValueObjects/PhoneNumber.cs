using System.Text.RegularExpressions;
using Paschoalotto.People.Domain.Common;


namespace Paschoalotto.People.Domain.People.ValueObjects;

public sealed class PhoneNumber : ValueObject
{
    public string Value { get; }

    private PhoneNumber() { Value = string.Empty; } // EF
    public PhoneNumber(string value)
    {
        var digits = Regex.Replace(value ?? string.Empty, "[^0-9+]", "").Trim();
        if (string.IsNullOrWhiteSpace(digits))
            throw new ArgumentException("Phone é obrigatório.", nameof(value));

        // Para PoC aceitamos dígitos com ou sem +55; validações avançadas podem vir depois.
        Value = digits;
    }

    public override string ToString() => Value;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
