using Paschoalotto.People.Domain.People;

namespace Paschoalotto.People.Application.Abstractions.Repositories;

public interface IPersonWriteRepository
{
    Task AddIndividualAsync(Individual individual, CancellationToken ct = default);
    Task AddLegalEntityAsync(LegalEntity legalEntity, CancellationToken ct = default);
    Task<bool> UpdateIndividualPhotoAsync(Guid id, string relativePath, CancellationToken ct = default);
    Task<bool> UpdateLegalEntityLogoAsync(Guid id, string relativePath, CancellationToken ct = default);

}
