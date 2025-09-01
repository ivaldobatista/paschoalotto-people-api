using Microsoft.Extensions.Configuration;
using Paschoalotto.People.Application.Abstractions;

namespace Paschoalotto.People.Infrastructure.Storage;

public sealed class FileSystemStorageService : IFileStorageService
{
    private readonly string _root;
    private readonly long _maxSize;
    private static readonly HashSet<string> _allowed = new(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png" };

    public FileSystemStorageService(IConfiguration cfg)
    {
        _root = cfg.GetSection("Storage")["Root"] ?? "./_storage";
        Directory.CreateDirectory(_root);

        var max = cfg.GetSection("Storage")["MaxFileSizeBytes"];
        _maxSize = long.TryParse(max, out var parsed) ? parsed : 5 * 1024 * 1024;
    }

    public async Task<string> SaveAsync(Stream content, string extensionWithDot, string category, string fileNamePrefix, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(extensionWithDot)) throw new ArgumentException("Extensão obrigatória.", nameof(extensionWithDot));
        if (!_allowed.Contains(extensionWithDot)) throw new InvalidOperationException("Extensão de arquivo não permitida. Use .jpg, .jpeg ou .png.");

        if (!content.CanRead) throw new InvalidOperationException("Stream inválida.");
        if (content.CanSeek) content.Position = 0;
        if (content.CanSeek && content.Length > _maxSize)
            throw new InvalidOperationException($"Arquivo excede o tamanho máximo permitido ({_maxSize} bytes).");

        category = category?.Trim().ToLowerInvariant() ?? "misc";
        var categoryDir = Path.Combine(_root, category);
        Directory.CreateDirectory(categoryDir);

        var safePrefix = (fileNamePrefix ?? "file").ToLowerInvariant().Replace(" ", "-");
        var fileName = $"{safePrefix}-{Guid.NewGuid():N}{extensionWithDot.ToLowerInvariant()}";
        var fullPath = Path.Combine(categoryDir, fileName);

        using (var fs = new FileStream(fullPath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
        {
            await content.CopyToAsync(fs, ct);
        }

        return $"{category}/{fileName}".Replace("\\", "/");
    }
}
