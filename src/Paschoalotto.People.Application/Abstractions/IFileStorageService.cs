namespace Paschoalotto.People.Application.Abstractions;

public interface IFileStorageService
{
    /// <summary>
    /// Salva um arquivo binário e retorna o caminho relativo padronizado (para persistir no DB).
    /// </summary>
    /// <param name="content">Stream já posicionado em 0.</param>
    /// <param name="extensionWithDot">Ex.: ".jpg", ".png".</param>
    /// <param name="category">Ex.: "photos" ou "logos".</param>
    /// <param name="fileNamePrefix">Prefixo para nomear o arquivo (ex.: "individual-{guid}").</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <returns>Path relativo, ex.: "photos/individual-<guid>.jpg".</returns>
    Task<string> SaveAsync(Stream content, string extensionWithDot, string category, string fileNamePrefix, CancellationToken ct = default);
}
