namespace Furaffinity.Client.Contracts;

/// <summary>
/// User comment in submission.
/// </summary>
public class UserComment
{
    internal UserComment(string? author, string? text)
    {
        if (string.IsNullOrWhiteSpace(author))
        {
            throw new ArgumentNullException(nameof(author));
        }
        
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentNullException(nameof(text));
        }

        AuthorName = author;
        Text = text;
    }
    
    /// <summary>
    /// Comment author.
    /// </summary>
    public string AuthorName { get; }

    /// <summary>
    /// Comment text.
    /// </summary>
    public string Text { get; }
}