using ClarusAI.Core.Entities;

namespace ClarusAI.Core.Interfaces;

public interface IAuditService
{
    Task LogActionAsync(Guid userId, string action, string entityType, string entityId, string details, string ipAddress, string userAgent);
    Task<IEnumerable<AuditLog>> GetUserAuditLogsAsync(Guid userId, DateTime? from = null, DateTime? to = null);
    Task<IEnumerable<AuditLog>> GetEntityAuditLogsAsync(string entityType, string entityId);
    Task<IEnumerable<AuditLog>> GetSystemAuditLogsAsync(DateTime? from = null, DateTime? to = null);
}