using Paschoalotto.People.Domain.People;

namespace Paschoalotto.People.Application.Abstractions.Repositories;

public interface IPersonReadRepository
{
    Task<Person?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Person>> SearchByNameAsync(string name, int take = 50, CancellationToken ct = default);

}
