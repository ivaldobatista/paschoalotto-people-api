namespace Paschoalotto.People.Application.Abstractions;

public interface IAuditLogger
{
    void Log(string action, string entityType, string entityId, object? extra = null);
}
