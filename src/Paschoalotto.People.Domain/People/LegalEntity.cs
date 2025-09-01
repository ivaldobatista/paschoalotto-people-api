using Paschoalotto.People.Domain.People.ValueObjects;

namespace Paschoalotto.People.Domain.People;

public sealed class LegalEntity : Person
{
    public string CorporateName { get; private set; } = null!;
    public string TradeName { get; private set; } = null!;
    public Cnpj Cnpj { get; private set; } = null!;
    public string? StateRegistration { get; private set; }
    public string? MunicipalRegistration { get; private set; }
    public string LegalRepresentativeName { get; private set; } = null!;
    public Cpf LegalRepresentativeCpf { get; private set; } = null!;
    public string? LogoPath { get; private set; }

    private LegalEntity() { } // EF

    public LegalEntity(
        string corporateName, string tradeName, Cnpj cnpj,
        string? stateRegistration, string? municipalRegistration,
        string legalRepName, Cpf legalRepCpf,
        EmailAddress email, PhoneNumber phone, Address address)
    {
        CorporateName = corporateName;
        TradeName = tradeName;
        Cnpj = cnpj;
        StateRegistration = stateRegistration;
        MunicipalRegistration = municipalRegistration;
        LegalRepresentativeName = legalRepName;
        LegalRepresentativeCpf = legalRepCpf;
        Email = email;
        Phone = phone;
        Address = address;
    }

    public void UpdateLogo(string? path)
    {
        LogoPath = path;
        Touch();
    }
}
