namespace Furaffinity.Client.Models;

/// <summary>
/// Submission put in scraps wrapper.
/// </summary>
public class PutInScrap
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
}