using System.ComponentModel.DataAnnotations;

namespace ISIncidentTracker.Web.Models;

/// <summary>
/// Категория инцидента информационной безопасности.
/// </summary>
public class Category
{
    /// <summary>
    /// Уникальный идентификатор категории.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Наименование категории (например, "Утечка данных", "DDoS-атака").
    /// </summary>
    [Required(ErrorMessage = "Наименование категории обязательно")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Длина от 3 до 100 символов")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Описание категории инцидента.
    /// </summary>
    [StringLength(500, ErrorMessage = "Описание не должно превышать 500 символов")]
    public string? Description { get; set; }

    /// <summary>
    /// Код категории для системной интеграции.
    /// </summary>
    [StringLength(50)]
    public string? Code { get; set; }

    /// <summary>
    /// Навигационное свойство: коллекция инцидентов данной категории.
    /// </summary>
    public ICollection<Incident>? Incidents { get; set; }
}
