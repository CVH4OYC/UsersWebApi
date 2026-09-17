using Microsoft.AspNetCore.Mvc;
using UserWebApi.Models.Common;
using UserWebApi.Models.Dto;
using UserWebApi.Services;

namespace UserWebApi.Controllers;

/// <summary>
/// Контроллер для управления справочником пользователей.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="UsersController"/>.
    /// </summary>
    /// <param name="userService">Сервис бизнес-логики пользователей.</param>
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Создать нового пользователя.
    /// </summary>
    /// <param name="request">Данные нового пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Ответ с созданным пользователем.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        var id = await _userService.CreateUserAsync(request, cancellationToken);
        return CreatedAtRoute(nameof(GetUserByIdAsync), new { id }, new { id });
    }

    /// <summary>
    /// Получить список пользователей с пагинацией и фильтрацией.
    /// </summary>
    /// <param name="filter">Параметры фильтрации.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список пользователей.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUsersAsync([FromQuery] UserFilterRequest filter, CancellationToken cancellationToken)
    {
        var result = await _userService.GetUsersAsync(filter, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Получить пользователя по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Пользователь с указанным идентификатором.</returns>
    [HttpGet("{id}", Name = nameof(GetUserByIdAsync))]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByIdAsync(id, cancellationToken);
        return Ok(user);
    }

    /// <summary>
    /// Обновить существующего пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя для обновления.</param>
    /// <param name="request">Новые данные пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Обновленный пользователь.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateUserAsync(Guid id, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var updatedUser = await _userService.UpdateUserAsync(id, request, cancellationToken);
        return Ok(updatedUser);
    }

    /// <summary>
    /// Удалить пользователя по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор удаляемого пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Пустой ответ при успешном удалении.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUserAsync(Guid id, CancellationToken cancellationToken)
    {
        await _userService.DeleteUserAsync(id, cancellationToken);
        return NoContent();
    }
}
