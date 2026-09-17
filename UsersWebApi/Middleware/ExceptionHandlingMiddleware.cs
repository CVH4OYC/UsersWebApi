using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Text.Json;
using UserWebApi.Exceptions;

namespace UserWebApi.Middleware;

/// <summary>
/// Middleware для централизованной обработки исключений.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="ExceptionHandlingMiddleware"/>.
    /// </summary>
    /// <param name="next">Следующий делегат в пайплайне.</param>
    /// <param name="logger">Логгер.</param>
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Обработать HTTP-запрос и передать его дальше по конвейеру.
    /// </summary>
    /// <param name="context">Контекст HTTP-запроса.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Ошибка валидации при обработке запроса.");
            await HandleValidationExceptionAsync(context, ex);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogWarning(ex, "Сущность не найдена.");
            await HandleNotFoundExceptionAsync(context, ex);
        }
        catch (PostgresException ex) when (ex.SqlState == PostgresErrorCodes.UniqueViolation)
        {
            _logger.LogWarning(ex, "Нарушено ограничение уникальности при обработке запроса.");
            await HandleConflictExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Необработанное исключение при выполнении запроса.");
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Сформировать ответ с ошибками валидации.
    /// </summary>
    /// <param name="context">Контекст HTTP-запроса.</param>
    /// <param name="exception">Исключение с ошибками валидации.</param>
    /// <returns>Задача записи ответа клиенту.</returns>
    private static async Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        var problemDetails = new ValidationProblemDetails(exception.Errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Ошибка валидации",
            Detail = exception.Message
        };

        var json = JsonSerializer.Serialize(problemDetails);
        await context.Response.WriteAsync(json);
    }

    /// <summary>
    /// Сформировать ответ о ненайденном ресурсе.
    /// </summary>
    /// <param name="context">Контекст HTTP-запроса.</param>
    /// <param name="exception">Исключение о ненайденной сущности.</param>
    /// <returns>Задача записи ответа клиенту.</returns>
    private static async Task HandleNotFoundExceptionAsync(HttpContext context, EntityNotFoundException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status404NotFound;

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Ресурс не найден",
            Detail = exception.Message
        };

        var json = JsonSerializer.Serialize(problemDetails);
        await context.Response.WriteAsync(json);
    }

    /// <summary>
    /// Сформировать ответ о конфликте данных.
    /// </summary>
    /// <param name="context">Контекст HTTP-запроса.</param>
    /// <param name="exception">Исключение PostgreSQL о нарушении уникальности.</param>
    /// <returns>Задача записи ответа клиенту.</returns>
    private static async Task HandleConflictExceptionAsync(HttpContext context, PostgresException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status409Conflict;

        var detail = exception.ConstraintName == "UX_Users_Email_Lower"
            ? "Пользователь с таким email уже существует."
            : "Запись с такими данными уже существует.";

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Title = "Конфликт данных",
            Detail = detail
        };

        var json = JsonSerializer.Serialize(problemDetails);
        await context.Response.WriteAsync(json);
    }

    /// <summary>
    /// Сформировать ответ о внутренней ошибке сервера.
    /// </summary>
    /// <param name="context">Контекст HTTP-запроса.</param>
    /// <param name="exception">Необработанное исключение.</param>
    /// <returns>Задача записи ответа клиенту.</returns>
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Внутренняя ошибка сервера",
            Detail = "Произошла непредвиденная ошибка при обработке запроса."
        };

        var json = JsonSerializer.Serialize(problemDetails);
        await context.Response.WriteAsync(json);
    }
}
