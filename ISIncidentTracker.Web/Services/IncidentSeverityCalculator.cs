using ISIncidentTracker.Web.Models;
using ISIncidentTracker.Web.Models.Enums;

namespace ISIncidentTracker.Web.Services;

/// <summary>
/// Сервис для автоматического расчёта уровня критичности инцидента на основе категории.
/// </summary>
public class IncidentSeverityCalculator
{
    /// <summary>
    /// Вычисляет уровень критичности по названию категории (гибкое сравнение).
    /// </summary>
    /// <param name="category">Категория инцидента.</param>
    /// <returns>Уровень критичности (IncidentSeverity).</returns>
    public IncidentSeverity CalculateSeverity(Category? category)
    {
        if (category == null || string.IsNullOrWhiteSpace(category.Name))
            return IncidentSeverity.Medium;

        // Приводим к нижнему регистру и убираем лишние пробелы для надёжного сравнения
        var name = category.Name.Trim().ToLowerInvariant();

        // === CRITICAL (Критический) ===
        if (name.Contains("утечка") ||
            name.Contains("ransomware") ||
            name.Contains("несанкционированный доступ") ||
            name.Contains("взлом") ||
            name.Contains("компрометация"))
            return IncidentSeverity.Critical;

        // === HIGH (Высокий) ===
        if (name.Contains("ddos") ||
            name.Contains("вредоносное по") ||
            name.Contains("malware") ||
            name.Contains("sqli") ||
            name.Contains("sql injection") ||
            name.Contains("шифровальщик"))
            return IncidentSeverity.High;

        // === MEDIUM (Средний) ===
        if (name.Contains("фишинг") ||
            name.Contains("xss") ||
            name.Contains("социальная инженерия") ||
            name.Contains("спам") ||
            name.Contains("подозрительная активность"))
            return IncidentSeverity.Medium;

        // === LOW (Низкий) — по умолчанию ===
        return IncidentSeverity.Low;
    }

    /// <summary>
    /// Возвращает текстовое описание уровня критичности.
    /// </summary>
    public static string GetSeverityDescription(IncidentSeverity severity)
    {
        return severity switch
        {
            IncidentSeverity.Critical => "Критический: Немедленное реагирование",
            IncidentSeverity.High => "Высокий: Реагирование в течение 1 часа",
            IncidentSeverity.Medium => "Средний: Реагирование в течение 4 часов",
            IncidentSeverity.Low => "Низкий: Реагирование в течение 24 часов",
            _ => "Не определено"
        };
    }

    /// <summary>
    /// Возвращает CSS-класс Bootstrap для стилизации бейджа уровня критичности.
    /// </summary>
    public static string GetSeverityCssClass(IncidentSeverity severity)
    {
        return severity switch
        {
            IncidentSeverity.Critical => "bg-danger",
            IncidentSeverity.High => "bg-orange",
            IncidentSeverity.Medium => "bg-warning text-dark",
            IncidentSeverity.Low => "bg-success",
            _ => "bg-secondary"
        };
    }
}
