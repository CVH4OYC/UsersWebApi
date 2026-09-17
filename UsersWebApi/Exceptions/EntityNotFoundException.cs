namespace UserWebApi.Exceptions;

/// <summary>
/// Исключение, выбрасываемое при обращении к несуществующей сущности.
/// </summary>
public class EntityNotFoundException : Exception
{
    /// <summary>
    /// Имя типа сущности.
    /// </summary>
    public string EntityName { get; }

    /// <summary>
    /// Идентификатор искомой сущности.
    /// </summary>
    public object EntityId { get; }

    /// <param name="entityName">Имя типа сущности.</param>
    /// <param name="entityId">Идентификатор искомой сущности.</param>
    public EntityNotFoundException(string entityName, object entityId)
        : base($"{entityName} с идентификатором '{entityId}' не найден.")
    {
        EntityName = entityName;
        EntityId = entityId;
    }
}
