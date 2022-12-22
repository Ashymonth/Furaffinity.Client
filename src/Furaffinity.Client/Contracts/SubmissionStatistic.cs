namespace Furaffinity.Client.Contracts;

/// <summary>
/// Current statistic for submission.
/// </summary>
public class SubmissionStatistic
{
    /// <summary>
    /// Views.
    /// </summary>
    public int Views { get; init; }

    /// <summary>
    /// Comments.
    /// </summary>
    public int Comments { get; init; }

    /// <summary>
    /// Favorites.
    /// </summary>
    public int Favorites { get; init; }

    /// <summary>
    /// Comments, that users wrote.
    /// </summary>
    public IList<UserComment> UserComments { get; set; } = new List<UserComment>();

    internal void AddUserComment(UserComment comment) => UserComments.Add(comment);
}