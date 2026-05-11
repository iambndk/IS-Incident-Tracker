using System.ComponentModel.DataAnnotations;

namespace ISIncidentTracker.Web.Models;

/// <summary>
/// Пользователь системы учета инцидентов.
/// </summary>
public class User
{
    /// <summary>
    /// Уникальный идентификатор пользователя.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Логин пользователя для входа в систему.
    /// </summary>
    [Required(ErrorMessage = "Логин обязателен")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Логин от 3 до 50 символов")]
    [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "Недопустимые символы в логине")]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Полное имя пользователя (ФИО).
    /// </summary>
    [StringLength(150, ErrorMessage = "ФИО не должно превышать 150 символов")]
    public string? FullName { get; set; }

    /// <summary>
    /// Адрес электронной почты пользователя.
    /// </summary>
    [EmailAddress(ErrorMessage = "Некорректный формат email")]
    [StringLength(100, ErrorMessage = "Email не должен превышать 100 символов")]
    public string? Email { get; set; }

    /// <summary>
    /// Роль пользователя в системе (Admin, Analyst, Viewer).
    /// </summary>
    [StringLength(50)]
    public string? Role { get; set; } = "Viewer";

    /// <summary>
    /// Дата и время создания записи пользователя.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Дата и время последнего входа пользователя.
    /// </summary>
    public DateTime? LastLoginAt { get; set; }

    /// <summary>
    /// Флаг активности пользователя.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Навигационное свойство: инциденты, созданные пользователем.
    /// </summary>
    public ICollection<Incident>? ReportedIncidents { get; set; }

    /// <summary>
    /// Навигационное свойство: инциденты, назначенные пользователю.
    /// </summary>
    public ICollection<Incident>? AssignedIncidents { get; set; }
}
