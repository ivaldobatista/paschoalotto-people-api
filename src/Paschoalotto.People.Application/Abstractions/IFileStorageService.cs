namespace Paschoalotto.People.Application.Abstractions;

public interface IFileStorageService
{
    Task<string> SaveAsync(Stream content, string extensionWithDot, string category, string fileNamePrefix, CancellationToken ct = default);
}
