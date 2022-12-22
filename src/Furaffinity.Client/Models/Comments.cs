namespace Furaffinity.Client.Models;

/// <summary>
/// Submission comments wrapper.
/// </summary>
public class Comments
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
}