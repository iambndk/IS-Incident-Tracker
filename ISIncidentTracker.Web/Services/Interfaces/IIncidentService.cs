using ISIncidentTracker.Web.Models;
using ISIncidentTracker.Web.Models.Enums;

namespace ISIncidentTracker.Web.Services.Interfaces;

/// <summary>
/// Интерфейс сервиса для управления инцидентами.
/// </summary>
public interface IIncidentService
{
    Task<Incident> CreateIncidentAsync(Incident incident);
    Task<IEnumerable<Incident>> GetAllIncidentsAsync();
    Task<Incident?> GetIncidentByIdAsync(int id);
    Task<Incident> UpdateIncidentAsync(Incident incident);
    Task DeleteIncidentAsync(int id);
    Task<Incident> AssignIncidentAsync(int incidentId, int userId);
    Task<Incident> ChangeIncidentStatusAsync(int incidentId, IncidentStatus newStatus, string? resolution = null);
    Task<Dictionary<IncidentSeverity, int>> GetSeverityStatisticsAsync();
}
