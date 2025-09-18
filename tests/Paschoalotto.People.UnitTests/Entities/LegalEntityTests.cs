using System;
using FluentAssertions;
using Paschoalotto.People.Domain.People;
using Paschoalotto.People.Domain.People.ValueObjects;
using Xunit;


namespace Paschoalotto.People.UnitTests.Entities;

public class LegalEntityTests
{
    [Fact]
    public void UpdateLogo_should_set_path()
    {
        var j = new LegalEntity(
            corporateName: "Acme LTDA",
            tradeName: "Acme",
            cnpj: new Cnpj("00.504.288/0001-31"), // 00504288000131
            stateRegistration: "ISENTO",
            municipalRegistration: "123",
            legalRepName: "João Souza",
            legalRepCpf: new Cpf("11144477735"),
            email: new EmailAddress("contato@acme.com"),
            phone: new PhoneNumber("+556199999"),
            address: new Address("Av. B", "100", "Sala 3", "Centro", "Brasília", "DF", "70000-000", "Brasil")
        );

        j.UpdateLogo("logos/acme.jpg");

        j.LogoPath.Should().Be("logos/acme.jpg");
    }

    [Fact]
    public void Ctor_should_initialize_core_properties()
    {
        var cnpj = new Cnpj("00504288000131");
        var cpf = new Cpf("11144477735");

        var j = new LegalEntity(
            corporateName: "Acme LTDA",
            tradeName: "Acme",
            cnpj: cnpj,
            stateRegistration: null,
            municipalRegistration: null,
            legalRepName: "João Souza",
            legalRepCpf: cpf,
            email: new EmailAddress("contato@acme.com"),
            phone: new PhoneNumber("+556199999"),
            address: new Address("Av. B", "100", null, "Centro", "Brasília", "DF", "70000-000", "Brasil")
        );

        j.CorporateName.Should().Be("Acme LTDA");
        j.TradeName.Should().Be("Acme");
        j.Cnpj.Value.Should().Be("00504288000131");
        j.LegalRepresentativeName.Should().Be("João Souza");
        j.LegalRepresentativeCpf.Value.Should().Be("11144477735");
        j.Email.Value.Should().Be("contato@acme.com");
        j.Phone.Value.Should().Be("+556199999");
        j.Address.City.Should().Be("Brasília");
    }

    [Theory]
    [InlineData("00000000000000")]
    [InlineData("string")]
    [InlineData("")]
    public void Ctor_should_throw_on_invalid_cnpj(string invalid)
    {
        var act = () => new LegalEntity(
            corporateName: "Acme LTDA",
            tradeName: "Acme",
            cnpj: new Cnpj(invalid),
            stateRegistration: null,
            municipalRegistration: null,
            legalRepName: "João Souza",
            legalRepCpf: new Cpf("11144477735"),
            email: new EmailAddress("contato@acme.com"),
            phone: new PhoneNumber("+556199999"),
            address: new Address("Av. B", "100", null, "Centro", "Brasília", "DF", "70000-000", "Brasil")
        );

        act.Should().Throw<ArgumentException>()
           .WithMessage("*CNPJ inválido*");
    }
}
