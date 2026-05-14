using ISIncidentTracker.Web.Models;
using ISIncidentTracker.Web.Models.Enums;

namespace ISIncidentTracker.Web.Data.Repositories.Interfaces;

/// <summary>
/// Интерфейс репозитория для работы с инцидентами.
/// </summary>
public interface IIncidentRepository : IRepository<Incident>
{
    /// <summary>
    /// Получает все инциденты с включенными связанными данными.
    /// </summary>
    Task<IEnumerable<Incident>> GetAllWithIncludesAsync();

    /// <summary>
    /// Получает статистику инцидентов по уровню критичности.
    /// </summary>
    Task<Dictionary<IncidentSeverity, int>> GetSeverityStatisticsAsync();

    /// <summary>
    /// Получает инциденты по статусу.
    /// </summary>
    Task<IEnumerable<Incident>> GetByStatusAsync(IncidentStatus status);

    /// <summary>
    /// Получает инциденты по диапазону дат.
    /// </summary>
    Task<IEnumerable<Incident>> GetByDateRangeAsync(DateTime from, DateTime to);
}
