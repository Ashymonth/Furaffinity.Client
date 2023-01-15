namespace Furaffinity.Client.Models;

/// <summary>
/// Submission gender wrapper.
/// </summary>
public class Gender : IEqualityComparer<Gender>
{
    private const string Template = "Gender: {0} not valid gender";

    private static readonly Dictionary<string, string> NameToIdMap = new()
    {
        {"any", "0"},
        {"female", "3"},
        {"herm", "4"},
        {"intersex", "11"},
        {"male", "2"},
        {"multiple characters", "6"},
        {"non-binary", "10"},
        {"other / not specified", "7"},
        {"trans (female)", "9"},
        {"trans (male)", "8"},
    };

    internal Gender(string genderName)
    {
        if (string.IsNullOrWhiteSpace(genderName))
        {
            throw new ArgumentNullException(nameof(genderName));
        }

        if (!NameToIdMap.TryGetValue(genderName.ToLower(), out var id))
        {
            throw new InvalidOperationException(string.Format(Template, genderName));
        }

        GenderId = id;
    }

    /// <summary>
    /// Gender id.
    /// </summary>
    public string GenderId { get; }

    /// <inheritdoc />
    public bool Equals(Gender? x, Gender? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.GenderId == y.GenderId;
    }

    /// <inheritdoc />
    public int GetHashCode(Gender obj)
    {
        return obj.GenderId.GetHashCode();
    }
}