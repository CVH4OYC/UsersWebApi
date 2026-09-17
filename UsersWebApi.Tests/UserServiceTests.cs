using Microsoft.Extensions.Logging;
using Moq;
using UserWebApi.Exceptions;
using UserWebApi.Models.Dto;
using UserWebApi.Models.Entities;
using UserWebApi.Repositories;
using UserWebApi.Services;
using UserWebApi.Validators;
using ValidationException = UserWebApi.Exceptions.ValidationException;

namespace UsersWebApi.Tests;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ILogger<UserService>> _loggerMock;
    private readonly CreateUserRequestValidator _createValidator;
    private readonly UpdateUserRequestValidator _updateValidator;
    private readonly UserFilterRequestValidator _filterValidator;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _loggerMock = new Mock<ILogger<UserService>>();
        _createValidator = new CreateUserRequestValidator();
        _updateValidator = new UpdateUserRequestValidator();
        _filterValidator = new UserFilterRequestValidator();

        _userService = new UserService(
            _userRepositoryMock.Object,
            _createValidator,
            _updateValidator,
            _filterValidator,
            _loggerMock.Object);
    }

    [Fact]
    public async Task CreateUserAsync_WithValidData_ReturnsCreatedUserAndLogsAstraMessage()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            FirstName = "Иван",
            LastName = "Петров",
            Email = "ivan@example.com",
            BirthDate = new DateOnly(1990, 1, 1),
            AstraSource = "astra_system_1",
            StatusId = 1
        };

        _userRepositoryMock.Setup(repo => repo.CheckStatusExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _userRepositoryMock.Setup(repo => repo.AddUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User user, CancellationToken ct) => user);

        // Act
        var resultId = await _userService.CreateUserAsync(request, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, resultId);

        _userRepositoryMock.Verify(repo => repo.AddUserAsync(It.Is<User>(u =>
            u.FirstName == request.FirstName &&
            u.LastName == request.LastName &&
            u.Email == request.Email &&
            u.AstraSource == request.AstraSource &&
            u.StatusId == request.StatusId), It.IsAny<CancellationToken>()), Times.Once);

        // Проверка логирования
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("[astra] пользователь успешно создан в системе astra")),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }

    [Fact]
    public async Task CreateUserAsync_WithInvalidData_ThrowsValidationException()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            FirstName = "", // Ошибка валидации
            LastName = "Петров",
            Email = "invalid_email", // Ошибка валидации
            BirthDate = new DateOnly(1990, 1, 1),
            AstraSource = "astra_system_1",
            StatusId = 1
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(() =>
            _userService.CreateUserAsync(request, CancellationToken.None));

        Assert.Contains(nameof(request.FirstName), exception.Errors.Keys);
        Assert.Contains(nameof(request.Email), exception.Errors.Keys);
    }

    [Fact]
    public async Task GetUserByIdAsync_WhenUserDoesNotExist_ThrowsEntityNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserDto?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _userService.GetUserByIdAsync(userId, CancellationToken.None));

        Assert.Equal(nameof(User), exception.EntityName);
        Assert.Equal(userId, exception.EntityId);
    }
}

