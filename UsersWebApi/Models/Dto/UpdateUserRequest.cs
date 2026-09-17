namespace UserWebApi.Models.Dto;

/// <summary>
/// Запрос на обновление пользователя.
/// </summary>
public class UpdateUserRequest
{
    /// <summary>
    /// Имя.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Фамилия.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Электронная почта.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Дата рождения.
    /// </summary>
    public DateOnly BirthDate { get; set; }

    /// <summary>
    /// Источник Astra.
    /// </summary>
    public string AstraSource { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор статуса.
    /// </summary>
    public int StatusId { get; set; }
}
