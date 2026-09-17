using FluentValidation;
using UserWebApi.Models.Dto;

namespace UserWebApi.Validators;

/// <summary>
/// Валидатор для параметров фильтрации пользователей.
/// </summary>
public class UserFilterRequestValidator : AbstractValidator<UserFilterRequest>
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="UserFilterRequestValidator"/>.
    /// </summary>
    public UserFilterRequestValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1).WithMessage("Номер страницы должен быть больше или равен 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Размер страницы должен быть от 1 до 100.");
    }
}
