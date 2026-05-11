using ISIncidentTracker.Web.Data.Repositories.Interfaces;
using ISIncidentTracker.Web.Models;
using ISIncidentTracker.Web.Models.Enums;
using ISIncidentTracker.Web.Services.Interfaces;

namespace ISIncidentTracker.Web.Services;

/// <summary>
/// Сервис для управления инцидентами информационной безопасности.
/// </summary>
public class IncidentService : IIncidentService
{
    private readonly IIncidentRepository _incidentRepository;
    private readonly IRepository<User> _userRepository;

    public IncidentService(IIncidentRepository incidentRepository, IRepository<User> userRepository)
    {
        _incidentRepository = incidentRepository ?? throw new ArgumentNullException(nameof(incidentRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<Incident> CreateIncidentAsync(Incident incident)
    {
        if (incident == null) throw new ArgumentNullException(nameof(incident));

        incident.ReportedDate = DateTime.UtcNow;
        incident.Status = IncidentStatus.New;
        incident.UpdatedAt = DateTime.UtcNow;

        await _incidentRepository.AddAsync(incident);
        await _incidentRepository.SaveChangesAsync();

        return incident;
    }

    public async Task<IEnumerable<Incident>> GetAllIncidentsAsync()
    {
        return await _incidentRepository.GetAllWithIncludesAsync();
    }

    public async Task<Incident?> GetIncidentByIdAsync(int id)
    {
        return await _incidentRepository.GetByIdAsync(id);
    }

    public async Task<Incident> UpdateIncidentAsync(Incident incident)
    {
        if (incident == null) throw new ArgumentNullException(nameof(incident));

        var existing = await _incidentRepository.GetByIdAsync(incident.Id);
        if (existing == null) throw new InvalidOperationException("Инцидент не найден");

        incident.UpdatedAt = DateTime.UtcNow;
        await _incidentRepository.UpdateAsync(incident);
        await _incidentRepository.SaveChangesAsync();

        return incident;
    }

    public async Task DeleteIncidentAsync(int id)
    {
        await _incidentRepository.DeleteAsync(id);
        await _incidentRepository.SaveChangesAsync();
    }

    public async Task<Incident> AssignIncidentAsync(int incidentId, int userId)
    {
        var incident = await _incidentRepository.GetByIdAsync(incidentId);
        if (incident == null) throw new InvalidOperationException("Инцидент не найден");

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) throw new InvalidOperationException("Пользователь не найден");

        incident.AssignedToId = userId;
        incident.Status = IncidentStatus.Assigned;
        incident.UpdatedAt = DateTime.UtcNow;

        await _incidentRepository.UpdateAsync(incident);
        await _incidentRepository.SaveChangesAsync();

        return incident;
    }

    public async Task<Incident> ChangeIncidentStatusAsync(int incidentId, IncidentStatus newStatus, string? resolution = null)
    {
        var incident = await _incidentRepository.GetByIdAsync(incidentId);
        if (incident == null) throw new InvalidOperationException("Инцидент не найден");

        incident.Status = newStatus;
        incident.UpdatedAt = DateTime.UtcNow;

        if (newStatus == IncidentStatus.Resolved || newStatus == IncidentStatus.Closed)
        {
            incident.ResolvedDate = DateTime.UtcNow;
            incident.Resolution = resolution;
        }

        await _incidentRepository.UpdateAsync(incident);
        await _incidentRepository.SaveChangesAsync();

        return incident;
    }

    public async Task<Dictionary<IncidentSeverity, int>> GetSeverityStatisticsAsync()
    {
        return await _incidentRepository.GetSeverityStatisticsAsync();
    }
}
