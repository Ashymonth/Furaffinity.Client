using Furaffinity.Client.Constants;
using Furaffinity.Client.Exceptions;

namespace Furaffinity.Client.Models;

/// <summary>
/// Submission rating wrapper.
/// </summary>
public class Rating : IEqualityComparer<Rating>
{
    private const string Template = "Rating: {0} is not valid submission rating";
    
    private static readonly Dictionary<string, string> NameToIdMap = new()
    {
        {SubmissionRatingName.General.ToLower(), "0"},
        {SubmissionRatingName.Mature.ToLower(), "2"},
        {SubmissionRatingName.Adult.ToLower(), "1"},
    };

    internal Rating(string ratingName)
    {
        if (string.IsNullOrWhiteSpace(ratingName))
        {
            throw new ArgumentNullException(nameof(ratingName));
        }

        if (!NameToIdMap.TryGetValue(ratingName.ToLower(), out var id))
        {
            throw new InvalidOperationException(string.Format(Template, ratingName));
        }

        RatingId = id;
    }
    
    /// <summary>
    /// Rating id.
    /// </summary>
    public string RatingId { get; }

    /// <inheritdoc />
    public bool Equals(Rating? x, Rating? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.RatingId == y.RatingId;
    }

    /// <inheritdoc />
    public int GetHashCode(Rating obj)
    {
        return obj.RatingId.GetHashCode();
    }
}