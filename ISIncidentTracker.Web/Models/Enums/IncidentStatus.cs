namespace ISIncidentTracker.Web.Models.Enums;

/// <summary>
/// Статус обработки инцидента информационной безопасности.
/// </summary>
public enum IncidentStatus
{
    /// <summary>
    /// Инцидент зарегистрирован, ожидает назначения.
    /// </summary>
    New = 1,

    /// <summary>
    /// Инцидент назначен ответственному специалисту.
    /// </summary>
    Assigned = 2,

    /// <summary>
    /// Ведется расследование и анализ инцидента.
    /// </summary>
    InProgress = 3,

    /// <summary>
    /// Инцидент ожидает решения или подтверждения.
    /// </summary>
    Pending = 4,

    /// <summary>
    /// Инцидент разрешен, применяются меры защиты.
    /// </summary>
    Resolved = 5,

    /// <summary>
    /// Инцидент закрыт после проверки и документирования.
    /// </summary>
    Closed = 6,

    /// <summary>
    /// Инцидент отклонен как ложное срабатывание.
    /// </summary>
    Rejected = 7
}
