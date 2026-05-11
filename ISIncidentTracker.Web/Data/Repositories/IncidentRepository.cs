using Microsoft.EntityFrameworkCore;
using ISIncidentTracker.Web.Models;
using ISIncidentTracker.Web.Models.Enums;
using ISIncidentTracker.Web.Data.Repositories.Interfaces;

namespace ISIncidentTracker.Web.Data.Repositories;

/// <summary>
/// Реализация репозитория для работы с инцидентами.
/// </summary>
public class IncidentRepository : Repository<Incident>, IIncidentRepository
{
    public IncidentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Incident>> GetAllWithIncludesAsync()
    {
        // Загружаем связанные данные: Category, ReportedBy, AssignedTo
        return await _dbSet
            .Include(i => i.Category)
            .Include(i => i.ReportedBy)
            .Include(i => i.AssignedTo)
            .OrderByDescending(i => i.ReportedDate)
            .ToListAsync();
    }

    public async Task<Dictionary<IncidentSeverity, int>> GetSeverityStatisticsAsync()
    {
        return await _dbSet
            .GroupBy(i => i.Severity)
            .Select(g => new { Severity = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Severity, x => x.Count);
    }
}
