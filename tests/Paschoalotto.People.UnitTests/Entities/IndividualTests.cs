using FluentAssertions;
using Paschoalotto.People.Domain.People;
using Paschoalotto.People.Domain.People.Enums;
using Paschoalotto.People.Domain.People.ValueObjects;

namespace Paschoalotto.People.UnitTests.Entities;

public class IndividualTests
{
    [Fact]
    public void UpdatePhoto_should_set_path()
    {
        var i = new Individual("Maria", new Cpf("11144477735"),
            new DateTime(1990, 5, 12), GenderType.Female,
            new EmailAddress("maria@example.com"), new PhoneNumber("+556199999"),
            new Address("Rua A", "10", null, "Centro", "Brasília", "DF", "70000-000", "Brasil"));

        i.UpdatePhoto("photos/x.jpg");
        i.PhotoPath.Should().Be("photos/x.jpg");
    }
}
