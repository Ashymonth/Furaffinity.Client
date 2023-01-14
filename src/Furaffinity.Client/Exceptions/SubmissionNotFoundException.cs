namespace Furaffinity.Client.Exceptions;

/// <summary>
/// The exception that is thrown if submission not found
/// </summary>
public class SubmissionNotFoundException : FuraffinityException
{
    /// <summary>
    /// Create a new instance of <see cref="SubmissionNotFoundException"/>
    /// </summary>
    public SubmissionNotFoundException(string message) : base(message)
    {
        
    }
}