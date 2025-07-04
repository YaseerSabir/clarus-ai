using ClarusAI.Core.Entities;
using ClarusAI.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace ClarusAI.Business.Services;

public class AuditService : IAuditService
{
    private readonly IRepository<AuditLog> _auditRepository;
    private readonly ILogger<AuditService> _logger;

    public AuditService(IRepository<AuditLog> auditRepository, ILogger<AuditService> logger)
    {
        _auditRepository = auditRepository;
        _logger = logger;
    }

    public async Task LogActionAsync(Guid userId, string action, string entityType, string entityId, string details, string ipAddress, string userAgent)
    {
        try
        {
            var auditLog = new AuditLog
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Action = action,
                EntityType = entityType,
                EntityId = entityId,
                Description = details,
                Details = details,
                OldValues = string.Empty,
                NewValues = string.Empty,
                IpAddress = ipAddress ?? string.Empty,
                UserAgent = userAgent ?? string.Empty,
                Timestamp = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            await _auditRepository.AddAsync(auditLog);
            await _auditRepository.SaveChangesAsync();

            _logger.LogInformation("Audit log created for user {UserId}, action {Action}", userId, action);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating audit log for user {UserId}, action {Action}", userId, action);
        }
    }

    public async Task<IEnumerable<AuditLog>> GetUserAuditLogsAsync(Guid userId, DateTime? from = null, DateTime? to = null)
    {
        try
        {
            var allLogs = await _auditRepository.GetAllAsync();
            var filteredLogs = allLogs.Where(x => x.UserId == userId);

            if (from.HasValue)
                filteredLogs = filteredLogs.Where(x => x.CreatedAt >= from.Value);

            if (to.HasValue)
                filteredLogs = filteredLogs.Where(x => x.CreatedAt <= to.Value);

            return filteredLogs.OrderByDescending(x => x.CreatedAt).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user audit logs for {UserId}", userId);
            return [];
        }
    }

    public async Task<IEnumerable<AuditLog>> GetEntityAuditLogsAsync(string entityType, string entityId)
    {
        try
        {
            var allLogs = await _auditRepository.GetAllAsync();
            return allLogs.Where(x => x.EntityType == entityType && x.EntityId == entityId)
                         .OrderByDescending(x => x.CreatedAt)
                         .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving entity audit logs for {EntityType}:{EntityId}", entityType, entityId);
            return [];
        }
    }

    public async Task<IEnumerable<AuditLog>> GetSystemAuditLogsAsync(DateTime? from = null, DateTime? to = null)
    {
        try
        {
            var allLogs = await _auditRepository.GetAllAsync();
            var filteredLogs = allLogs.AsQueryable();

            if (from.HasValue)
                filteredLogs = filteredLogs.Where(x => x.CreatedAt >= from.Value);

            if (to.HasValue)
                filteredLogs = filteredLogs.Where(x => x.CreatedAt <= to.Value);

            return filteredLogs.OrderByDescending(x => x.CreatedAt).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving system audit logs");
            return [];
        }
    }
}