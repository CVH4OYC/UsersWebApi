using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UserWebApi.Controllers;
using UserWebApi.Models.Dto;
using UserWebApi.Services;

namespace UsersWebApi.Tests;

public class UsersControllerTests
{
    [Fact]
    public async Task CreateUserAsync_ReturnsCreatedRouteWithUserId()
    {
        var userId = Guid.NewGuid();
        var serviceMock = new Mock<IUserService>();
        var request = new CreateUserRequest();

        serviceMock
            .Setup(service => service.CreateUserAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userId);

        var controller = new UsersController(serviceMock.Object);

        var result = await controller.CreateUserAsync(request, CancellationToken.None);

        var createdResult = Assert.IsType<CreatedAtRouteResult>(result);
        Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
        Assert.Equal(nameof(UsersController.GetUserByIdAsync), createdResult.RouteName);
        Assert.Equal(userId, createdResult.RouteValues!["id"]);
    }

    [Fact]
    public void GetUserByIdAsync_UsesNamedRoute()
    {
        var method = typeof(UsersController).GetMethod(nameof(UsersController.GetUserByIdAsync));
        var routeAttribute = method?.GetCustomAttribute<HttpGetAttribute>();

        Assert.NotNull(routeAttribute);
        Assert.Equal(nameof(UsersController.GetUserByIdAsync), routeAttribute!.Name);
    }
}
