using Paschoalotto.People.Application.Abstractions;
using Paschoalotto.People.Infrastructure.Persistence;

namespace Paschoalotto.People.Infrastructure;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly PeopleDbContext _db;

    public UnitOfWork(PeopleDbContext db) => _db = db;

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);
}
