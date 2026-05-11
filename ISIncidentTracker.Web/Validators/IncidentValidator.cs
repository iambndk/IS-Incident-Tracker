using FluentValidation;
using ISIncidentTracker.Web.Models;
using ISIncidentTracker.Web.Models.Enums;

namespace ISIncidentTracker.Web.Validators;

/// <summary>
/// Валидатор для сущности Incident.
/// </summary>
public class IncidentValidator : AbstractValidator<Incident>
{
    public IncidentValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Заголовок обязателен")
            .MinimumLength(5).WithMessage("Минимум 5 символов")
            .MaximumLength(200).WithMessage("Максимум 200 символов");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Описание обязательно")
            .MinimumLength(20).WithMessage("Минимум 20 символов")
            .MaximumLength(2000).WithMessage("Максимум 2000 символов");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Выберите категорию");

        RuleFor(x => x.ReportedById)
            .GreaterThan(0).WithMessage("Выберите автора");

        RuleFor(x => x.Severity)
            .IsInEnum().WithMessage("Недопустимое значение критичности");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Недопустимое значение статуса");

        RuleFor(x => x.Resolution)
            .MaximumLength(2000).WithMessage("Решение не должно превышать 2000 символов")
            .When(x => !string.IsNullOrWhiteSpace(x.Resolution));

        // Бизнес-правило: нельзя закрыть инцидент без решения
        RuleFor(x => x.Resolution)
            .NotEmpty().WithMessage("Для закрытия инцидента необходимо указать решение")
            .When(x => x.Status == IncidentStatus.Closed || x.Status == IncidentStatus.Resolved);
    }
}
