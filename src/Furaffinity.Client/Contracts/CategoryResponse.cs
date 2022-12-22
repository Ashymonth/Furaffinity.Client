namespace Furaffinity.Client.Contracts;

/// <summary>
/// Category element.
/// </summary>
public class CategoryResponse
{
    /// <summary>
    /// Category name.
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// Category value
    /// </summary>
    public string? Value { get; init; }
}