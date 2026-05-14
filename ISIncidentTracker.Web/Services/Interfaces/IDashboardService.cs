using ISIncidentTracker.Web.Models;
using ISIncidentTracker.Web.Models.Enums;

namespace ISIncidentTracker.Web.Services.Interfaces;

/// <summary>
/// Интерфейс сервиса для получения данных дашборда.
/// </summary>
public interface IDashboardService
{
    /// <summary>
    /// Получает KPI для дашборда.
    /// </summary>
    Task<DashboardKpi> GetKpiAsync();

    /// <summary>
    /// Получает полную статистику для дашборда.
    /// </summary>
    Task<DashboardStatistics> GetStatisticsAsync();

    /// <summary>
    /// Получает количество инцидентов по статусу.
    /// </summary>
    Task<Dictionary<IncidentStatus, int>> GetIncidentsByStatusAsync();

    /// <summary>
    /// Получает количество инцидентов по категориям.
    /// </summary>
    Task<Dictionary<string, int>> GetIncidentsByCategoryAsync();

    /// <summary>
    /// Получает последние инциденты.
    /// </summary>
    Task<IEnumerable<Incident>> GetRecentIncidentsAsync(int count = 10);
}
