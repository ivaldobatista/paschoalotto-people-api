using Microsoft.EntityFrameworkCore;
using Paschoalotto.People.Application.Abstractions.Repositories;
using Paschoalotto.People.Domain.People;
using Paschoalotto.People.Infrastructure.Persistence;

namespace Paschoalotto.People.Infrastructure.Repositories;

public sealed class PersonWriteRepository : IPersonWriteRepository
{
    private readonly PeopleDbContext _db;

    public PersonWriteRepository(PeopleDbContext db) => _db = db;

    public Task AddIndividualAsync(Individual individual, CancellationToken ct = default)
    {
        _db.Individuals.Add(individual);
        return Task.CompletedTask;
    }

    public Task AddLegalEntityAsync(LegalEntity legalEntity, CancellationToken ct = default)
    {
        _db.LegalEntities.Add(legalEntity);
        return Task.CompletedTask;
    }

    public async Task<bool> UpdateIndividualPhotoAsync(Guid id, string relativePath, CancellationToken ct = default)
    {
        var person = await _db.Individuals.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (person is null) return false;
        person.UpdatePhoto(relativePath);
        return true;
    }

    public async Task<bool> UpdateLegalEntityLogoAsync(Guid id, string relativePath, CancellationToken ct = default)
    {
        var person = await _db.LegalEntities.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (person is null) return false;
        person.UpdateLogo(relativePath);
        return true;
    }

}
