using ISIncidentTracker.Web.Models;
using ISIncidentTracker.Web.Models.Enums;

namespace ISIncidentTracker.Web.Data.Repositories.Interfaces;

/// <summary>
/// Интерфейс репозитория для работы с инцидентами.
/// </summary>
public interface IIncidentRepository : IRepository<Incident>
{
    /// <summary>
    /// Получает инциденты с включенными связанными данными (Eager Loading).
    /// </summary>
    Task<IEnumerable<Incident>> GetAllWithIncludesAsync();

    /// <summary>
    /// Получает статистику инцидентов по критичности.
    /// </summary>
    Task<Dictionary<IncidentSeverity, int>> GetSeverityStatisticsAsync();
}
