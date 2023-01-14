namespace Furaffinity.Client.Models;

/// <summary>
/// Submission title wrapper.
/// </summary>
public class Title : IEqualityComparer<Title>
{
    private const int TitleMaxLength = 512;
    
    internal Title(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentNullException(nameof(title));
        }

        if (title.Length > TitleMaxLength)
        {
            throw new ArgumentException("Title can't be more that 512 characters", nameof(title));
        }

        Value = title;
    }

    /// <summary>
    /// Title value.
    /// </summary>
    public string Value { get;  }

    /// <inheritdoc />
    public bool Equals(Title? x, Title? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.Value == y.Value;
    }

    /// <inheritdoc />
    public int GetHashCode(Title obj)
    {
        return obj.Value.GetHashCode();
    }
}