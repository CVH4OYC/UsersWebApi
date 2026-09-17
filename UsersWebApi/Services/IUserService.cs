using UserWebApi.Models.Common;
using UserWebApi.Models.Dto;

namespace UserWebApi.Services;

/// <summary>
/// Интерфейс бизнес-логики пользователей.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Создать нового пользователя.
    /// </summary>
    /// <param name="request">Данные для создания пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Идентификатор созданного пользователя.</returns>
    Task<Guid> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Получить пагинированный список пользователей.
    /// </summary>
    /// <param name="filter">Параметры фильтрации.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Пагинированный результат со списком пользователей.</returns>
    Task<PagedResult<UserDto>> GetUsersAsync(UserFilterRequest filter, CancellationToken cancellationToken);

    /// <summary>
    /// Получить пользователя по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>DTO пользователя.</returns>
    Task<UserDto> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Обновить пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="request">Данные для обновления.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Обновленный пользователь.</returns>
    Task<UserDto> UpdateUserAsync(Guid id, UpdateUserRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Удалить пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    Task DeleteUserAsync(Guid id, CancellationToken cancellationToken);
}
