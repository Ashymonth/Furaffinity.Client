namespace Furaffinity.Client.Models;

/// <summary>
/// Submission comments wrapper.
/// </summary>
public class Comments : IEqualityComparer<Comments>
{
    private const string EnabledId = "1";
    private const string DisabledId = "0";
    
    private Comments()
    {
    }

    /// <summary>
    /// Action id.
    /// </summary>
    public string Id { get; private init; } = null!;

    /// <summary>
    /// Enable comments for submission.
    /// </summary>
    /// <returns></returns>
    public static Comments Enabled() => new() {Id = EnabledId};
    
    /// <summary>
    /// Disable comments for submission.
    /// </summary>
    /// <returns></returns>
    public static Comments Disabled() => new() {Id = DisabledId};
    
    /// <inheritdoc />
    public bool Equals(Comments? x, Comments? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.Id == y.Id;
    }

    /// <inheritdoc />
    public int GetHashCode(Comments obj)
    {
        return obj.Id.GetHashCode();
    }
}