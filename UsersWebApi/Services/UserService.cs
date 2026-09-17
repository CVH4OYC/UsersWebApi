using FluentValidation;
using UserWebApi.Exceptions;
using ValidationException = UserWebApi.Exceptions.ValidationException;
using UserWebApi.Models.Common;
using UserWebApi.Models.Dto;
using UserWebApi.Models.Entities;
using UserWebApi.Repositories;

namespace UserWebApi.Services;

/// <summary>
/// Реализация сервиса управления пользователями.
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<CreateUserRequest> _createValidator;
    private readonly IValidator<UpdateUserRequest> _updateValidator;
    private readonly IValidator<UserFilterRequest> _filterValidator;
    private readonly ILogger<UserService> _logger;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="UserService"/>.
    /// </summary>
    public UserService(
        IUserRepository userRepository,
        IValidator<CreateUserRequest> createValidator,
        IValidator<UpdateUserRequest> updateValidator,
        IValidator<UserFilterRequest> filterValidator,
        ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _filterValidator = filterValidator;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<Guid> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.ToDictionary());
        }

        var statusExists = await _userRepository.CheckStatusExistsAsync(request.StatusId, cancellationToken);
        if (!statusExists)
        {
            throw new ValidationException(new Dictionary<string, string[]> 
            { 
                { nameof(request.StatusId), new[] { "Указанный статус не существует." } } 
            });
        }

        var now = DateTimeOffset.UtcNow;
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            BirthDate = request.BirthDate,
            CreatedAt = now,
            UpdatedAt = now,
            AstraSource = request.AstraSource,
            StatusId = request.StatusId
        };

        var createdUser = await _userRepository.AddUserAsync(user, cancellationToken);

        _logger.LogInformation("[astra] пользователь успешно создан в системе astra Id: {Id}", createdUser.Id);

        return createdUser.Id;
    }

    /// <inheritdoc />
    public async Task<PagedResult<UserDto>> GetUsersAsync(UserFilterRequest filter, CancellationToken cancellationToken)
    {
        var validationResult = await _filterValidator.ValidateAsync(filter, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.ToDictionary());
        }

        var (totalCount, items) = await _userRepository.GetUsersAsync(filter, cancellationToken);

        return new PagedResult<UserDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }

    /// <inheritdoc />
    public async Task<UserDto> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(id, cancellationToken);
        if (user == null)
        {
            throw new EntityNotFoundException(nameof(User), id);
        }

        return user;
    }

    /// <inheritdoc />
    public async Task<UserDto> UpdateUserAsync(Guid id, UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.ToDictionary());
        }

        var exists = await _userRepository.CheckUserExistsAsync(id, cancellationToken);
        if (!exists)
        {
            throw new EntityNotFoundException(nameof(User), id);
        }
        
        var statusExists = await _userRepository.CheckStatusExistsAsync(request.StatusId, cancellationToken);
        if (!statusExists)
        {
            throw new ValidationException(new Dictionary<string, string[]> 
            { 
                { nameof(request.StatusId), new[] { "Указанный статус не существует." } } 
            });
        }

        var now = DateTimeOffset.UtcNow;
        var user = new User
        {
            Id = id,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            BirthDate = request.BirthDate,
            UpdatedAt = now,
            AstraSource = request.AstraSource,
            StatusId = request.StatusId
        };

        await _userRepository.UpdateUserAsync(user, cancellationToken);

        return await GetUserByIdAsync(id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task DeleteUserAsync(Guid id, CancellationToken cancellationToken)
    {
        var exists = await _userRepository.CheckUserExistsAsync(id, cancellationToken);
        if (!exists)
        {
            throw new EntityNotFoundException(nameof(User), id);
        }

        await _userRepository.DeleteUserAsync(id, cancellationToken);
    }
}

