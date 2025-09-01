using Microsoft.EntityFrameworkCore;
using Paschoalotto.People.Domain.People;
using Paschoalotto.People.Infrastructure.Persistence;
using Paschoalotto.People.Application.Abstractions.Repositories;

namespace Paschoalotto.People.Infrastructure.Repositories;

public sealed class PersonReadRepository : IPersonReadRepository
{
    private readonly PeopleDbContext _db;

    public PersonReadRepository(PeopleDbContext db) => _db = db;

    public async Task<Person?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var i = await _db.Individuals.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
        if (i is not null) return i;
        var j = await _db.LegalEntities.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
        return j;
    }

    public async Task<IReadOnlyList<Person>> SearchByNameAsync(string name, int take = 50, CancellationToken ct = default)
    {
        name = name?.Trim() ?? string.Empty;
        var left = await _db.Individuals.AsNoTracking()
            .Where(x => EF.Functions.Like(x.FullName, $"%{name}%"))
            .Take(take)
            .Cast<Person>()
            .ToListAsync(ct);

        var right = await _db.LegalEntities.AsNoTracking()
            .Where(x => EF.Functions.Like(x.CorporateName, $"%{name}%") || EF.Functions.Like(x.TradeName, $"%{name}%"))
            .Take(take)
            .Cast<Person>()
            .ToListAsync(ct);

        return left.Concat(right).Take(take).ToList();
    }
}
