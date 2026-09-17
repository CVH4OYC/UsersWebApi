namespace UserWebApi.Models.Dto;

/// <summary>
/// DTO для возврата пользователя.
/// </summary>
public class UserDto
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid Id { get; set; }

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
    /// Дата и время создания.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Дата и время последнего обновления.
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }

    /// <summary>
    /// Источник Astra.
    /// </summary>
    public string AstraSource { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор статуса.
    /// </summary>
    public int StatusId { get; set; }

    /// <summary>
    /// Название статуса.
    /// </summary>
    public string StatusName { get; set; } = string.Empty;
}
