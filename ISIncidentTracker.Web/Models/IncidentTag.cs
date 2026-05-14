namespace ISIncidentTracker.Web.Models;

/// <summary>
/// Промежуточная сущность для связи Incident ↔ Tag (Many-to-Many).
/// </summary>
public class IncidentTag
{
    /// <summary>
    /// Идентификатор инцидента.
    /// </summary>
    public int IncidentId { get; set; }

    /// <summary>
    /// Навигационное свойство: инцидент.
    /// </summary>
    public Incident? Incident { get; set; }

    /// <summary>
    /// Идентификатор тега.
    /// </summary>
    public int TagId { get; set; }

    /// <summary>
    /// Навигационное свойство: тег.
    /// </summary>
    public Tag? Tag { get; set; }
}
