
using Paschoalotto.People.Domain.Common;
using System.Net.Mail;


namespace Paschoalotto.People.Domain.People.ValueObjects;

public sealed class EmailAddress : ValueObject
{
    public string Value { get; }

    private EmailAddress() { Value = string.Empty; } // EF
    public EmailAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("E-mail é obrigatório.", nameof(value));

        try { _ = new MailAddress(value); }
        catch { throw new ArgumentException("E-mail inválido.", nameof(value)); }

        Value = value.Trim();
    }

    public override string ToString() => Value;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value.ToLowerInvariant();
    }

}