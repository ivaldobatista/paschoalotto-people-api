using System.Text.Json;
using Microsoft.Extensions.Logging;
using Paschoalotto.People.Application.Abstractions;

namespace Paschoalotto.People.Infrastructure.Logging;

public sealed class NLogAuditLogger : IAuditLogger
{
    private readonly ILogger<NLogAuditLogger> _logger;

    public NLogAuditLogger(ILogger<NLogAuditLogger> logger)
    {
        _logger = logger;
    }

    public void Log(string action, string entityType, string entityId, object? extra = null)
    {
        string? payload = extra is null ? null : JsonSerializer.Serialize(extra);

        using (_logger.BeginScope(new Dictionary<string, object?>
        {
            ["action"] = action,
            ["entityType"] = entityType,
            ["entityId"] = entityId,
            ["extra"] = payload
        }))
        {
            _logger.LogInformation("AUDIT {Action} {EntityType} {EntityId}", action, entityType, entityId);
        }
    }
}
