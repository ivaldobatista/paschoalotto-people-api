using Paschoalotto.People.Domain.Common;
using Paschoalotto.People.Domain.People.ValueObjects;

namespace Paschoalotto.People.Domain.People;

public abstract class Person : EntityBase
{
    public EmailAddress Email { get; protected set; } = null!;
    public PhoneNumber Phone { get; protected set; } = null!;
    public Address Address { get; protected set; } = null!;
}
