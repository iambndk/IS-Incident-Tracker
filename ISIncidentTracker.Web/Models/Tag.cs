using System.ComponentModel.DataAnnotations;

namespace ISIncidentTracker.Web.Models;

/// <summary>
/// Тег для классификации инцидентов.
/// </summary>
public class Tag
{
    /// <summary>
    /// Уникальный идентификатор тега.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название тега.
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Описание тега.
    /// </summary>
    [StringLength(200)]
    public string? Description { get; set; }

    /// <summary>
    /// Навигационное свойство: связи с инцидентами.
    /// </summary>
    public ICollection<IncidentTag>? IncidentTags { get; set; }
}
