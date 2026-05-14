using ISIncidentTracker.Web.Data.Repositories.Interfaces;
using ISIncidentTracker.Web.Models;
using ISIncidentTracker.Web.Models.Enums;

namespace ISIncidentTracker.Web.Services;

/// <summary>
/// Сервис для получения данных дашборда мониторинга инцидентов.
/// Предоставляет статистику, KPI и данные для графиков.
/// </summary>
public class DashboardService
{
    private readonly IIncidentRepository _incidentRepository;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DashboardService"/>.
    /// </summary>
    /// <param name="incidentRepository">Репозиторий инцидентов.</param>
    public DashboardService(IIncidentRepository incidentRepository)
    {
        _incidentRepository = incidentRepository ?? throw new ArgumentNullException(nameof(incidentRepository));
    }

    /// <summary>
    /// Получает общее количество инцидентов.
    /// </summary>
    /// <returns>Общее количество инцидентов.</returns>
    public async Task<int> GetTotalIncidentsAsync()
    {
        var incidents = await _incidentRepository.GetAllAsync();
        return incidents.Count();
    }

    /// <summary>
    /// Получает количество инцидентов по статусу.
    /// </summary>
    /// <returns>Словарь: статус → количество.</returns>
    public async Task<Dictionary<IncidentStatus, int>> GetIncidentsByStatusAsync()
    {
        var incidents = await _incidentRepository.GetAllAsync();
        return incidents
            .GroupBy(i => i.Status)
            .ToDictionary(g => g.Key, g => g.Count());
    }

    /// <summary>
    /// Получает количество инцидентов по уровню критичности.
    /// </summary>
    /// <returns>Словарь: уровень критичности → количество.</returns>
    public async Task<Dictionary<IncidentSeverity, int>> GetIncidentsBySeverityAsync()
    {
        return await _incidentRepository.GetSeverityStatisticsAsync();
    }

    /// <summary>
    /// Получает количество инцидентов по категориям.
    /// </summary>
    /// <returns>Словарь: название категории → количество.</returns>
    public async Task<Dictionary<string, int>> GetIncidentsByCategoryAsync()
    {
        var incidents = await _incidentRepository.GetAllWithIncludesAsync();
        return incidents
            .GroupBy(i => i.Category?.Name ?? "Не указана")
            .ToDictionary(g => g.Key, g => g.Count());
    }

    /// <summary>
    /// Получает последние N инцидентов.
    /// </summary>
    /// <param name="count">Количество инцидентов для получения.</param>
    /// <returns>Коллекция последних инцидентов, отсортированных по дате.</returns>
    public async Task<IEnumerable<Incident>> GetRecentIncidentsAsync(int count = 10)
    {
        var incidents = await _incidentRepository.GetAllWithIncludesAsync();
        return incidents
            .OrderByDescending(i => i.ReportedDate)
            .Take(count);
    }

    /// <summary>
    /// Получает данные для графика инцидентов по дням.
    /// </summary>
    /// <param name="days">Количество дней для анализа.</param>
    /// <returns>Словарь: дата → количество инцидентов.</returns>
    public async Task<Dictionary<DateTime, int>> GetIncidentsTimelineAsync(int days = 30)
    {
        var incidents = await _incidentRepository.GetAllAsync();
        var startDate = DateTime.UtcNow.Date.AddDays(-days);

        return incidents
            .Where(i => i.ReportedDate >= startDate)
            .GroupBy(i => i.ReportedDate.Date)
            .ToDictionary(g => g.Key, g => g.Count());
    }

    /// <summary>
    /// Рассчитывает среднее время решения инцидентов в часах.
    /// </summary>
    /// <returns>Среднее время решения в часах.</returns>
    public async Task<double> GetAverageResolutionTimeAsync()
    {
        var incidents = await _incidentRepository.GetAllAsync();
        var resolved = incidents.Where(i => i.ResolvedDate.HasValue && i.OccurredDate.HasValue);

        if (!resolved.Any())
        {
            return 0;
        }

        var totalHours = resolved.Sum(i => (i.ResolvedDate!.Value - i.OccurredDate!.Value).TotalHours);
        return totalHours / resolved.Count();
    }

    /// <summary>
    /// Получает количество инцидентов по ответственным.
    /// </summary>
    /// <returns>Словарь: имя ответственного → количество инцидентов.</returns>
    public async Task<Dictionary<string, int>> GetIncidentsByAssigneeAsync()
    {
        var incidents = await _incidentRepository.GetAllWithIncludesAsync();
        return incidents
            .GroupBy(i => i.AssignedTo?.FullName ?? "Не назначен")
            .ToDictionary(g => g.Key, g => g.Count());
    }

    /// <summary>
    /// Получает KPI для дашборда.
    /// </summary>
    /// <returns>Объект с основными показателями.</returns>
    public async Task<DashboardKpi> GetKpiAsync()
    {
        var total = await GetTotalIncidentsAsync();
        var bySeverity = await GetIncidentsBySeverityAsync();
        var byStatus = await GetIncidentsByStatusAsync();
        var avgTime = await GetAverageResolutionTimeAsync();

        return new DashboardKpi
        {
            TotalIncidents = total,
            CriticalCount = bySeverity.GetValueOrDefault(IncidentSeverity.Critical, 0),
            HighCount = bySeverity.GetValueOrDefault(IncidentSeverity.High, 0),
            MediumCount = bySeverity.GetValueOrDefault(IncidentSeverity.Medium, 0),
            LowCount = bySeverity.GetValueOrDefault(IncidentSeverity.Low, 0),
            NewCount = byStatus.GetValueOrDefault(IncidentStatus.New, 0),
            InProgressCount = byStatus.GetValueOrDefault(IncidentStatus.InProgress, 0),
            PendingCount = byStatus.GetValueOrDefault(IncidentStatus.Pending, 0),
            ResolvedCount = byStatus.GetValueOrDefault(IncidentStatus.Resolved, 0),
            ClosedCount = byStatus.GetValueOrDefault(IncidentStatus.Closed, 0),
            AverageResolutionHours = 0
        };
    }

    /// <summary>
    /// Получает статистику для графиков.
    /// </summary>
    /// <returns>Объект со всей статистикой для дашборда.</returns>
    public async Task<DashboardStatistics> GetStatisticsAsync()
    {
        return new DashboardStatistics
        {
            Kpi = await GetKpiAsync(),
            ByCategory = await GetIncidentsByCategoryAsync(),
            BySeverity = await GetIncidentsBySeverityAsync(),
            ByStatus = await GetIncidentsByStatusAsync(),
            Timeline = await GetIncidentsTimelineAsync(30),
            RecentIncidents = await GetRecentIncidentsAsync(10)
        };
    }
}

/// <summary>
/// Модель KPI для дашборда.
/// Содержит основные показатели эффективности системы.
/// </summary>
/// <summary>
/// Модель KPI для дашборда.
/// Содержит основные показатели эффективности системы.
/// </summary>
public class DashboardKpi
{
    /// <summary>
    /// Общее количество инцидентов.
    /// </summary>
    public int TotalIncidents { get; set; }

    /// <summary>
    /// Количество критических инцидентов.
    /// </summary>
    public int CriticalCount { get; set; }

    /// <summary>
    /// Количество инцидентов с высоким приоритетом.
    /// </summary>
    public int HighCount { get; set; }

    /// <summary>
    /// Количество инцидентов со средним приоритетом.
    /// </summary>
    public int MediumCount { get; set; }

    /// <summary>
    /// Количество инцидентов с низким приоритетом.
    /// </summary>
    public int LowCount { get; set; }

    /// <summary>
    /// Количество новых инцидентов.
    /// </summary>
    public int NewCount { get; set; }

    /// <summary>
    /// Количество инцидентов в работе.
    /// </summary>
    public int InProgressCount { get; set; }

    /// <summary>
    /// Количество инцидентов, ожидающих решения.
    /// </summary>
    public int PendingCount { get; set; }

    /// <summary>
    /// Количество решенных инцидентов.
    /// </summary>
    public int ResolvedCount { get; set; }

    /// <summary>
    /// Количество закрытых инцидентов.
    /// </summary>
    public int ClosedCount { get; set; }

    /// <summary>
    /// Среднее время решения в часах.
    /// </summary>
    public double AverageResolutionHours { get; set; }
}
public class DashboardStatistics
{
    /// <summary>
    /// Основные показатели KPI.
    /// </summary>
    public DashboardKpi Kpi { get; set; } = null!;

    /// <summary>
    /// Распределение по категориям.
    /// </summary>
    public Dictionary<string, int> ByCategory { get; set; } = new();

    /// <summary>
    /// Распределение по уровням критичности.
    /// </summary>
    public Dictionary<IncidentSeverity, int> BySeverity { get; set; } = new();

    /// <summary>
    /// Распределение по статусам.
    /// </summary>
    public Dictionary<IncidentStatus, int> ByStatus { get; set; } = new();

    /// <summary>
    /// Временная шкала инцидентов (дата → количество).
    /// </summary>
    public Dictionary<DateTime, int> Timeline { get; set; } = new();

    /// <summary>
    /// Последние инциденты.
    /// </summary>
    public IEnumerable<Incident> RecentIncidents { get; set; } = new List<Incident>();
}
