namespace Furaffinity.Client.Models;

/// <summary>
/// Submission id wrapper.
/// </summary>
public class SubmissionId
{
    internal const int MaxLength = 8;

    /// <summary>
    /// Create a new instance of <see cref="SubmissionId"/>
    /// </summary>
    /// <param name="submissionId">submission id</param>
    /// <exception cref="ArgumentNullException">If submission string is empty</exception>
    /// <exception cref="InvalidCastException">If submission length more that {MaxLength}</exception>
    public SubmissionId(string? submissionId)
    {
        if (string.IsNullOrWhiteSpace(submissionId))
        {
            throw new ArgumentNullException(nameof(submissionId));
        }

        if (submissionId.Length > MaxLength)
        {
            throw new InvalidCastException($"Submission id length can't be more that {MaxLength}");
        }

        Value = submissionId;
    }
    
    /// <summary>
    /// Submission id value.
    /// </summary>
    public string Value { get;  }
}