namespace Furaffinity.Client.Models;

/// <summary>
/// Submission id wrapper.
/// </summary>
public class SubmissionId : IEqualityComparer<SubmissionId>
{
    internal const int MaxLength = 8;

    /// <summary>
    /// Create a new instance of <see cref="SubmissionId"/>
    /// </summary>
    /// <param name="submissionId">submission id</param>
    /// <exception cref="ArgumentNullException">If submission string is empty</exception>
    /// <exception cref="InvalidCastException">If submission length more that {MaxLength}</exception>
    public SubmissionId(string submissionId)
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

    /// <inheritdoc />
    public bool Equals(SubmissionId? x, SubmissionId? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.Value == y.Value;
    }

    /// <inheritdoc />
    public int GetHashCode(SubmissionId obj)
    {
        return obj.Value.GetHashCode();
    }
}