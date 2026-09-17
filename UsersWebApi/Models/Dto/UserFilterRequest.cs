namespace UserWebApi.Models.Dto;

/// <summary>
/// Параметры фильтрации и пагинации списка пользователей.
/// </summary>
public class UserFilterRequest
{
    /// <summary>
    /// Номер страницы (по умолчанию 1).
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Размер страницы (по умолчанию 10).
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Имя (поиск по подстроке).
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Фамилия (поиск по подстроке).
    /// </summary>
    public string? LastName { get; set; }
}
