using Dapper;
using UserWebApi.Models.Dto;
using UserWebApi.Models.Entities;

namespace UserWebApi.Repositories;

/// <summary>
/// Реализация репозитория пользователей с использованием Dapper.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="UserRepository"/>.
    /// </summary>
    /// <param name="connectionFactory">Фабрика подключений к БД.</param>
    public UserRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    /// <inheritdoc />
    public async Task<User> AddUserAsync(User user, CancellationToken cancellationToken)
    {
        var sql = @"
            INSERT INTO ""Users"" (""Id"", ""FirstName"", ""LastName"", ""Email"", ""BirthDate"", ""CreatedAt"", ""UpdatedAt"", ""AstraSource"", ""StatusId"")
            VALUES (@Id, @FirstName, @LastName, @Email, @BirthDate, @CreatedAt, @UpdatedAt, @AstraSource, @StatusId);";

        using var connection = _connectionFactory.CreateConnection();
        var commandDefinition = new CommandDefinition(sql, user, cancellationToken: cancellationToken);
        await connection.ExecuteAsync(commandDefinition);

        return user;
    }

    /// <inheritdoc />
    public async Task<UserDto?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var sql = @"
            SELECT 
                u.""Id"", 
                u.""FirstName"", 
                u.""LastName"", 
                u.""Email"", 
                u.""BirthDate"", 
                u.""CreatedAt"", 
                u.""UpdatedAt"", 
                u.""AstraSource"", 
                u.""StatusId"",
                s.""Name"" AS ""StatusName""
            FROM ""Users"" AS u
            INNER JOIN ""UserStatuses"" AS s ON u.""StatusId"" = s.""Id""
            WHERE u.""Id"" = @Id;";

        using var connection = _connectionFactory.CreateConnection();
        var commandDefinition = new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<UserDto>(commandDefinition);
    }

    /// <inheritdoc />
    public async Task<(int TotalCount, IList<UserDto> Items)> GetUsersAsync(UserFilterRequest filter, CancellationToken cancellationToken)
    {
        var whereParts = new List<string>();
        var parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(filter.FirstName))
        {
            whereParts.Add("u.\"FirstName\" ILIKE @FirstName");
            parameters.Add("FirstName", $"%{filter.FirstName}%");
        }

        if (!string.IsNullOrWhiteSpace(filter.LastName))
        {
            whereParts.Add("u.\"LastName\" ILIKE @LastName");
            parameters.Add("LastName", $"%{filter.LastName}%");
        }

        var whereClause = whereParts.Count > 0 ? "WHERE " + string.Join(" AND ", whereParts) : string.Empty;

        var countSql = $"SELECT COUNT(1) FROM \"Users\" AS u {whereClause};";
        
        var selectSql = $@"
            SELECT 
                u.""Id"", 
                u.""FirstName"", 
                u.""LastName"", 
                u.""Email"", 
                u.""BirthDate"", 
                u.""CreatedAt"", 
                u.""UpdatedAt"", 
                u.""AstraSource"", 
                u.""StatusId"",
                s.""Name"" AS ""StatusName""
            FROM ""Users"" AS u
            INNER JOIN ""UserStatuses"" AS s ON u.""StatusId"" = s.""Id""
            {whereClause}
            ORDER BY u.""CreatedAt"" DESC
            LIMIT @PageSize OFFSET @Offset;";

        parameters.Add("PageSize", filter.PageSize);
        parameters.Add("Offset", (filter.Page - 1) * filter.PageSize);

        using var connection = _connectionFactory.CreateConnection();
        
        var countCommand = new CommandDefinition(countSql, parameters, cancellationToken: cancellationToken);
        var totalCount = await connection.ExecuteScalarAsync<int>(countCommand);

        if (totalCount == 0)
        {
            return (0, new List<UserDto>());
        }

        var selectCommand = new CommandDefinition(selectSql, parameters, cancellationToken: cancellationToken);
        var items = await connection.QueryAsync<UserDto>(selectCommand);

        return (totalCount, items.ToList());
    }

    /// <inheritdoc />
    public async Task UpdateUserAsync(User user, CancellationToken cancellationToken)
    {
        var sql = @"
            UPDATE ""Users""
            SET ""FirstName"" = @FirstName,
                ""LastName"" = @LastName,
                ""Email"" = @Email,
                ""BirthDate"" = @BirthDate,
                ""UpdatedAt"" = @UpdatedAt,
                ""AstraSource"" = @AstraSource,
                ""StatusId"" = @StatusId
            WHERE ""Id"" = @Id;";

        using var connection = _connectionFactory.CreateConnection();
        var commandDefinition = new CommandDefinition(sql, user, cancellationToken: cancellationToken);
        await connection.ExecuteAsync(commandDefinition);
    }

    /// <inheritdoc />
    public async Task DeleteUserAsync(Guid id, CancellationToken cancellationToken)
    {
        var sql = "DELETE FROM \"Users\" WHERE \"Id\" = @Id;";

        using var connection = _connectionFactory.CreateConnection();
        var commandDefinition = new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken);
        await connection.ExecuteAsync(commandDefinition);
    }

    /// <inheritdoc />
    public async Task<bool> CheckUserExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        var sql = "SELECT 1 FROM \"Users\" WHERE \"Id\" = @Id LIMIT 1;";

        using var connection = _connectionFactory.CreateConnection();
        var commandDefinition = new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken);
        var result = await connection.ExecuteScalarAsync<int?>(commandDefinition);
        
        return result.HasValue;
    }

    /// <inheritdoc />
    public async Task<bool> CheckStatusExistsAsync(int statusId, CancellationToken cancellationToken)
    {
        var sql = "SELECT 1 FROM \"UserStatuses\" WHERE \"Id\" = @StatusId LIMIT 1;";

        using var connection = _connectionFactory.CreateConnection();
        var commandDefinition = new CommandDefinition(sql, new { StatusId = statusId }, cancellationToken: cancellationToken);
        var result = await connection.ExecuteScalarAsync<int?>(commandDefinition);
        
        return result.HasValue;
    }
}
