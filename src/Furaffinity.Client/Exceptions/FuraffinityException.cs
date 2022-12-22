namespace Furaffinity.Client.Exceptions;

/// <summary>
/// Base client exception.
/// </summary>
public class FuraffinityException : Exception
{
    /// <summary>
    /// Create a new instance of <see cref="FuraffinityException"/>
    /// </summary>
    public FuraffinityException()
    {
        
    }

    /// <summary>
    /// Create a new instance of <see cref="FuraffinityException"/>
    /// </summary>
    /// <param name="message">Exception message.</param>
    public FuraffinityException(string message) : base(message)
    {
        
    }
}