namespace UserWebApi.Models.Common;

/// <summary>
/// Контейнер для пагинированного ответа.
/// </summary>
/// <typeparam name="T">Тип элементов.</typeparam>
public class PagedResult<T>
{
    /// <summary>
    /// Список элементов на текущей странице.
    /// </summary>
    public IList<T> Items { get; init; } = [];

    /// <summary>
    /// Общее количество элементов.
    /// </summary>
    public int TotalCount { get; init; }

    /// <summary>
    /// Номер текущей страницы.
    /// </summary>
    public int Page { get; init; }

    /// <summary>
    /// Размер страницы.
    /// </summary>
    public int PageSize { get; init; }

    /// <summary>
    /// Общее количество страниц.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}
