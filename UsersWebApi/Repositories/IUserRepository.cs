using UserWebApi.Models.Common;
using UserWebApi.Models.Dto;
using UserWebApi.Models.Entities;

namespace UserWebApi.Repositories;

/// <summary>
/// Репозиторий для управления пользователями в БД.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Создать нового пользователя в базе данных.
    /// </summary>
    /// <param name="user">Сущность пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Созданный пользователь с заполненным идентификатором.</returns>
    Task<User> AddUserAsync(User user, CancellationToken cancellationToken);

    /// <summary>
    /// Получить пользователя по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Пользователь со статусом или null.</returns>
    Task<UserDto?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Получить пагинированный и отфильтрованный список пользователей.
    /// </summary>
    /// <param name="filter">Параметры фильтрации.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Контейнер с пользователями и общим количеством.</returns>
    Task<(int TotalCount, IList<UserDto> Items)> GetUsersAsync(UserFilterRequest filter, CancellationToken cancellationToken);

    /// <summary>
    /// Обновить данные пользователя в базе данных.
    /// </summary>
    /// <param name="user">Сущность пользователя с обновленными данными.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    Task UpdateUserAsync(User user, CancellationToken cancellationToken);

    /// <summary>
    /// Удалить пользователя по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    Task DeleteUserAsync(Guid id, CancellationToken cancellationToken);
    
    /// <summary>
    /// Проверить существование пользователя по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>True, если пользователь существует, иначе false.</returns>
    Task<bool> CheckUserExistsAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Проверить существование статуса по его идентификатору.
    /// </summary>
    /// <param name="statusId">Идентификатор статуса.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>True, если статус существует, иначе false.</returns>
    Task<bool> CheckStatusExistsAsync(int statusId, CancellationToken cancellationToken);
}
