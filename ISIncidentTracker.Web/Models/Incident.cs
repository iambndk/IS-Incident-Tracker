using System.ComponentModel.DataAnnotations;
using ISIncidentTracker.Web.Models.Enums;

namespace ISIncidentTracker.Web.Models;

/// <summary>
/// Инцидент информационной безопасности.
/// Основная сущность системы учета.
/// </summary>
public class Incident
{
    /// <summary>
    /// Уникальный идентификатор инцидента.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Заголовок инцидента (краткое описание).
    /// </summary>
    [Required(ErrorMessage = "Заголовок инцидента обязателен")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "Заголовок от 5 до 200 символов")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Подробное описание инцидента.
    /// </summary>
    [Required(ErrorMessage = "Описание инцидента обязательно")]
    [StringLength(2000, MinimumLength = 20, ErrorMessage = "Описание от 20 до 2000 символов")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Дата и время регистрации инцидента.
    /// </summary>
    public DateTime ReportedDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Дата и время фактического возникновения инцидента (если известно).
    /// </summary>
    public DateTime? OccurredDate { get; set; }

    /// <summary>
    /// Срок решения инцидента.
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Уровень критичности инцидента.
    /// </summary>
    public IncidentSeverity Severity { get; set; } = IncidentSeverity.Medium;

    /// <summary>
    /// Текущий статус обработки инцидента.
    /// </summary>
    public IncidentStatus Status { get; set; } = IncidentStatus.New;

    // ==========================================
    // Отношения (Foreign Keys)
    // ==========================================

    /// <summary>
    /// Идентификатор категории инцидента (внешний ключ).
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Навигационное свойство: категория инцидента.
    /// </summary>
    public Category? Category { get; set; }

    /// <summary>
    /// Идентификатор пользователя, сообщившего об инциденте (внешний ключ).
    /// </summary>
    public int ReportedById { get; set; }

    /// <summary>
    /// Навигационное свойство: пользователь, сообщивший об инциденте.
    /// </summary>
    public User? ReportedBy { get; set; }

    /// <summary>
    /// Идентификатор пользователя, ответственного за обработку (внешний ключ, опционально).
    /// </summary>
    public int? AssignedToId { get; set; }

    /// <summary>
    /// Навигационное свойство: пользователь, ответственный за обработку.
    /// </summary>
    public User? AssignedTo { get; set; }

    /// <summary>
    /// Дата и время разрешения инцидента.
    /// </summary>
    public DateTime? ResolvedDate { get; set; }

    /// <summary>
    /// Решение и принятые меры по инциденту.
    /// </summary>
    [StringLength(2000, ErrorMessage = "Решение не должно превышать 2000 символов")]
    public string? Resolution { get; set; }

    /// <summary>
    /// Дополнительные данные инцидента в формате JSON.
    /// </summary>
    [StringLength(4000, ErrorMessage = "Доп. данные не должны превышать 4000 символов")]
    public string? AdditionalData { get; set; }

    /// <summary>
    /// Дата и время последнего обновления записи.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Навигационное свойство: теги инцидента.
    /// </summary>
    public ICollection<IncidentTag>? IncidentTags { get; set; }
}
