namespace Furaffinity.Client.Models;

/// <summary>
/// Submission title wrapper.
/// </summary>
public class Title
{
    private const int TitleMaxLength = 512;
    
    internal Title(string? title)
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
}