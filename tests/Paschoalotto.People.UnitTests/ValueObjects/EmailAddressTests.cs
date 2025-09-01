using FluentAssertions;
using Paschoalotto.People.Domain.People.ValueObjects;

public class EmailAddressTests
{
    [Theory]
    [InlineData("john.doe@example.com")]
    [InlineData("a@b.co")]
    public void Should_accept_valid_email(string input)
    {
        var email = new EmailAddress(input);
        email.Value.Should().Be(input);
    }

    [Theory]
    [InlineData("no-at-symbol")]
    [InlineData("")]
    public void Should_reject_invalid_email(string input)
    {
        var act = () => new EmailAddress(input);
        act.Should().Throw<ArgumentException>();
    }
}
