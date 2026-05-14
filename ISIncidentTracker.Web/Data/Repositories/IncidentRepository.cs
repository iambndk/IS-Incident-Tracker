using Microsoft.EntityFrameworkCore;
using ISIncidentTracker.Web.Models;
using ISIncidentTracker.Web.Models.Enums;
using ISIncidentTracker.Web.Data.Repositories.Interfaces;

namespace ISIncidentTracker.Web.Data.Repositories;

/// <summary>
/// Реализация репозитория для работы с инцидентами информационной безопасности.
/// </summary>
public class IncidentRepository : Repository<Incident>, IIncidentRepository
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="IncidentRepository"/>.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    public IncidentRepository(AppDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Получает все инциденты с включенными связанными данными (Eager Loading).
    /// </summary>
    /// <returns>Коллекция инцидентов с категориями и пользователями.</returns>
    public async Task<IEnumerable<Incident>> GetAllWithIncludesAsync()
    {
        return await _dbSet
            .Include(i => i.Category)
            .Include(i => i.ReportedBy)
            .Include(i => i.AssignedTo)
            .OrderByDescending(i => i.ReportedDate)
            .ToListAsync();
    }

    /// <summary>
    /// Получает статистику инцидентов по уровню критичности.
    /// </summary>
    /// <returns>Словарь: уровень критичности → количество.</returns>
    public async Task<Dictionary<IncidentSeverity, int>> GetSeverityStatisticsAsync()
    {
        return await _dbSet
            .GroupBy(i => i.Severity)
            .Select(g => new { Severity = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Severity, x => x.Count);
    }

    /// <summary>
    /// Получает инциденты по статусу.
    /// </summary>
    /// <param name="status">Статус инцидента.</param>
    /// <returns>Коллекция инцидентов с указанным статусом.</returns>
    public async Task<IEnumerable<Incident>> GetByStatusAsync(IncidentStatus status)
    {
        return await _dbSet
            .Include(i => i.Category)
            .Include(i => i.ReportedBy)
            .Where(i => i.Status == status)
            .OrderByDescending(i => i.ReportedDate)
            .ToListAsync();
    }

    /// <summary>
    /// Получает инциденты по диапазону дат.
    /// </summary>
    /// <param name="from">Начальная дата.</param>
    /// <param name="to">Конечная дата.</param>
    /// <returns>Коллекция инцидентов за указанный период.</returns>
    public async Task<IEnumerable<Incident>> GetByDateRangeAsync(DateTime from, DateTime to)
    {
        return await _dbSet
            .Include(i => i.Category)
            .Where(i => i.ReportedDate >= from && i.ReportedDate <= to)
            .OrderByDescending(i => i.ReportedDate)
            .ToListAsync();
    }
}
