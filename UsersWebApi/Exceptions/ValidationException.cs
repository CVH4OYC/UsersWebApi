namespace UserWebApi.Exceptions;

/// <summary>
/// Исключение, выбрасываемое при ошибках валидации.
/// </summary>
public class ValidationException : Exception
{
    /// <summary>
    /// Словарь с ошибками валидации.
    /// </summary>
    public IDictionary<string, string[]> Errors { get; }

    /// <param name="errors">Словарь с ошибками.</param>
    public ValidationException(IDictionary<string, string[]> errors)
        : base("Произошла одна или несколько ошибок валидации.")
    {
        Errors = errors;
    }
}
