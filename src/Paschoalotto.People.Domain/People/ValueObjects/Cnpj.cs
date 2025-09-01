using Paschoalotto.People.Domain.Common;
using System.Text.RegularExpressions;


namespace Paschoalotto.People.Domain.People.ValueObjects;

public sealed class Cnpj : ValueObject
{
    public string Value { get; }

    private Cnpj() { Value = string.Empty; } // EF
    public Cnpj(string value)
    {
        var digits = Normalize(value);
        if (!IsValid(digits))
            throw new ArgumentException("CNPJ inválido.", nameof(value));
        Value = digits;
    }

    public override string ToString() => Value;

    public static string Normalize(string input)
        => Regex.Replace(input ?? string.Empty, "[^0-9]", "");

    public static bool IsValid(string digits)
    {
        if (string.IsNullOrWhiteSpace(digits)) return false;
        digits = Normalize(digits);
        if (digits.Length != 14) return false;
        if (new string(digits[0], digits.Length) == digits) return false;

        int[] mult1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] mult2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        string temp = digits[..12];
        int sum = 0;
        for (int i = 0; i < 12; i++) sum += (temp[i] - '0') * mult1[i];
        int remainder = sum % 11;
        int d1 = remainder < 2 ? 0 : 11 - remainder;

        temp += d1.ToString();
        sum = 0;
        for (int i = 0; i < 13; i++) sum += (temp[i] - '0') * mult2[i];
        remainder = sum % 11;
        int d2 = remainder < 2 ? 0 : 11 - remainder;

        return digits.EndsWith($"{d1}{d2}");
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}