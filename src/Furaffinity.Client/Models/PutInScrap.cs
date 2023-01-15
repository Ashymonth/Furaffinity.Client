namespace Furaffinity.Client.Models;

/// <summary>
/// Submission put in scraps wrapper.
/// </summary>
public class PutInScrap : IEqualityComparer<PutInScrap>
{
    private const string EnabledId = "1";
    private const string DisabledId = "0";
    
    private PutInScrap()
    {
        
    }
    
    /// <summary>
    /// Action id.
    /// </summary>
    public string Id { get; private init; } = null!;

    /// <summary>
    /// Enable put in scraps.
    /// </summary>
    /// <returns></returns>
    public static PutInScrap Enabled() => new() {Id = EnabledId};
    
    /// <summary>
    /// Disable put in scrap.
    /// </summary>
    /// <returns></returns>
    public static PutInScrap Disabled() => new() {Id = DisabledId};

    /// <inheritdoc />
    public bool Equals(PutInScrap? x, PutInScrap? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.Id == y.Id;
    }

    /// <inheritdoc />
    public int GetHashCode(PutInScrap obj)
    {
        return obj.Id.GetHashCode();
    }
}