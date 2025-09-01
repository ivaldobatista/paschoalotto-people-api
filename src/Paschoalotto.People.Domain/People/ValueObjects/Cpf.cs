using Paschoalotto.People.Domain.Common;
using System.Text.RegularExpressions;

namespace Paschoalotto.People.Domain.People.ValueObjects;

public sealed class Cpf : ValueObject
{
    public string Value { get; }

    private Cpf() { Value = string.Empty; } // EF
    public Cpf(string value)
    {
        var digits = Normalize(value);
        if (!IsValid(digits))
            throw new ArgumentException("CPF inválido.", nameof(value));
        Value = digits;
    }

    public override string ToString() => Value;

    public static string Normalize(string input)
        => Regex.Replace(input ?? string.Empty, "[^0-9]", "");

    public static bool IsValid(string digits)
    {
        if (string.IsNullOrWhiteSpace(digits)) return false;
        digits = Normalize(digits);
        if (digits.Length != 11) return false;
        if (new string(digits[0], digits.Length) == digits) return false;

        int[] multiplicator1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicator2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        string temp = digits[..9];
        int sum = 0;
        for (int i = 0; i < 9; i++) sum += (temp[i] - '0') * multiplicator1[i];
        int remainder = sum % 11;
        int d1 = remainder < 2 ? 0 : 11 - remainder;

        temp += d1.ToString();
        sum = 0;
        for (int i = 0; i < 10; i++) sum += (temp[i] - '0') * multiplicator2[i];
        remainder = sum % 11;
        int d2 = remainder < 2 ? 0 : 11 - remainder;

        return digits.EndsWith($"{d1}{d2}");
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}