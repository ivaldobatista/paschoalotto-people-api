using Paschoalotto.People.Domain.People;
using Paschoalotto.People.Domain.People.Enums;
using Paschoalotto.People.Domain.People.ValueObjects;
using Paschoalotto.People.Infrastructure.Persistence;

namespace Paschoalotto.People.Infrastructure.Seed;

public static class SeedData
{
    public static async Task ApplyAsync(PeopleDbContext db)
    {
        if (!db.Individuals.Any())
        {
            db.Individuals.Add(new Individual(
                fullName: "Maria Silva",
                cpf: new Cpf("123.456.789-09"),
                birthDate: new DateTime(1990, 5, 12),
                gender: GenderType.Female,
                email: new EmailAddress("maria.silva@example.com"),
                phone: new PhoneNumber("+5561999990001"),
                address: new Address("Rua A", "10", null, "Centro", "Brasília", "DF", "70000-000", "Brasil")
            ));
        }

        if (!db.LegalEntities.Any())
        {
            db.LegalEntities.Add(new LegalEntity(
                corporateName: "Acme LTDA",
                tradeName: "Acme",
                cnpj: new Cnpj("12.345.678/0001-95"),
                stateRegistration: null,
                municipalRegistration: null,
                legalRepName: "João Souza",
                legalRepCpf: new Cpf("987.654.321-00"),
                email: new EmailAddress("contato@acme.com"),
                phone: new PhoneNumber("+5561999990002"),
                address: new Address("Av. B", "100", "Sala 3", "Asa Sul", "Brasília", "DF", "70000-100", "Brasil")
            ));
        }

        await db.SaveChangesAsync();
    }
}
