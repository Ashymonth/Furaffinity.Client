namespace Furaffinity.Client.Exceptions;

/// <summary>
/// Throw if limit to actions exceeded.
/// </summary>
public class FloodProtectionException : FuraffinityException
{
    /// <summary>
    /// Create a new instance of <see cref="FloodProtectionException"/>
    /// </summary>
    public FloodProtectionException()
    {
        
    }
}