using FluentValidation;
using UserWebApi.Models.Dto;

namespace UserWebApi.Validators;

/// <summary>
/// Валидатор для запроса на создание пользователя.
/// </summary>
public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="CreateUserRequestValidator"/>.
    /// </summary>
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Имя обязательно для заполнения.")
            .MaximumLength(100).WithMessage("Имя не может превышать 100 символов.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Фамилия обязательна для заполнения.")
            .MaximumLength(100).WithMessage("Фамилия не может превышать 100 символов.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Электронная почта обязательна для заполнения.")
            .EmailAddress().WithMessage("Указан некорректный адрес электронной почты.");

        RuleFor(x => x.BirthDate)
            .NotEmpty().WithMessage("Дата рождения обязательна для заполнения.")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today)).WithMessage("Дата рождения не может быть в будущем.");

        RuleFor(x => x.AstraSource)
            .NotEmpty().WithMessage("Поле AstraSource обязательно для заполнения.");

        RuleFor(x => x.StatusId)
            .InclusiveBetween(1, 3).WithMessage("Указан некорректный идентификатор статуса.");
    }
}
