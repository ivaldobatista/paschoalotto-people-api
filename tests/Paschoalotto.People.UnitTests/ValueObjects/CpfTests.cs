using FluentAssertions;
using Paschoalotto.People.Domain.People.ValueObjects;

public class CpfTests
{
    [Theory]
    [InlineData("123.456.789-09")]
    [InlineData("11144477735")]
    public void Should_create_valid_cpf(string input)
    {
        var cpf = new Cpf(input);
        cpf.Value.Should().BeOneOf("12345678909", "11144477735");
    }

    [Theory]
    [InlineData("string")]
    [InlineData("00000000000")]
    [InlineData("")]
    public void Should_throw_on_invalid_cpf(string input)
    {
        var act = () => new Cpf(input);
        act.Should().Throw<ArgumentException>()
           .WithMessage("*CPF inválido*");
    }
}
