using System.Data;
using Npgsql;

namespace UserWebApi.Repositories;

/// <summary>
/// Реализация фабрики для создания подключений к PostgreSQL.
/// </summary>
public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DbConnectionFactory"/>.
    /// </summary>
    /// <param name="configuration">Конфигурация приложения.</param>
    public DbConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Строка подключения 'DefaultConnection' не найдена.");
    }

    /// <inheritdoc />
    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}
