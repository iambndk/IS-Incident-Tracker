namespace ISIncidentTracker.Web.Models.Enums;

/// <summary>
/// Уровень критичности инцидента информационной безопасности.
/// </summary>
public enum IncidentSeverity
{
    /// <summary>
    /// Низкий приоритет: не влияет на работу систем.
    /// </summary>
    Low = 1,

    /// <summary>
    /// Средний приоритет: частичное влияние на функциональность.
    /// </summary>
    Medium = 2,

    /// <summary>
    /// Высокий приоритет: критическое влияние на безопасность.
    /// </summary>
    High = 3,

    /// <summary>
    /// Критический приоритет: полная компрометация системы.
    /// </summary>
    Critical = 4
}
