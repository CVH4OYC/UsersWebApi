namespace UserWebApi.Models.Entities;

/// <summary>
/// Сущность статуса пользователя.
/// </summary>
public class UserStatus
{
    /// <summary>
    /// Идентификатор статуса.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название статуса.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}
