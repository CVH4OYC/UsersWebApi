using System.Data;

namespace UserWebApi.Repositories;

/// <summary>
/// Фабрика для создания подключений к базе данных.
/// </summary>
public interface IDbConnectionFactory
{
    /// <summary>
    /// Создать новое подключение к базе данных.
    /// </summary>
    /// <returns>Подключение <see cref="IDbConnection"/>.</returns>
    IDbConnection CreateConnection();
}
