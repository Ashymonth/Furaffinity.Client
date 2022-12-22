namespace Furaffinity.Client.Exceptions;

/// <summary>
/// Throw if unable to authorize on site
/// </summary>
public class AuthorizationCookieException : FuraffinityException
{
    /// <summary>
    /// Create new instance of <see cref="AuthorizationCookieException"/>
    /// </summary>
    /// <param name="message">Exception message.</param>
    public AuthorizationCookieException(string message) : base(message)
    {
        
    }
}