namespace Furaffinity.Client.Models;

/// <summary>
/// Submission theme wrapper.
/// </summary>
public class Theme : IEqualityComparer<Theme>
{
    private const string Template = "Theme: {0} not valid theme";
    
    private static readonly Dictionary<string, string> NameToIdMap = new()
    {
        {"60s","207"},
        {"70s","206"},
        {"80s","205"},
        {"90s","204"},
        {"abstract","2"},
        {"all","1"},
        {"animal related (non-anthro)","3"},
        {"anime","4"},
        {"baby fur","101"},
        {"bondage","102"},
        {"classical","209"},
        {"comics","5"},
        {"digimon","103"},
        {"doodle","6"},
        {"fanart","7"},
        {"fantasy","8"},
        {"fat furs","104"},
        {"fetish other","105"},
        {"fursuit","106"},
        {"game music","210"},
        {"general furry art","100"},
        {"gore / macabre art","119"},
        {"house","203"},
        {"human","9"},
        {"hyper","107"},
        {"hypnosis","121"},
        {"industrial","214"},
        {"inflation","108"},
        {"macro / micro","109"},
        {"miscellaneous","14"},
        {"muscle","110"},
        {"my little pony / brony","111"},
        {"other music","200"},
        {"paw","112"},
        {"pokemon","113"},
        {"pop","212"},
        {"portraits","10"},
        {"pre-60s","208"},
        {"pregnancy","114"},
        {"rap","213"},
        {"rock","211"},
        {"scenery","11"},
        {"sonic","115"},
        {"still life","12"},
        {"techno","201"},
        {"tf / tg","120"},
        {"trance","202"},
        {"transformation","116"},
        {"tutorials","13"},
        {"vore","117"},
        {"water sports","118"},

    };

    internal Theme(string themeName)
    {
        if (string.IsNullOrWhiteSpace(themeName))
        {
            throw new ArgumentNullException(nameof(themeName));
        }

        if (!NameToIdMap.TryGetValue(themeName.ToLower(), out var id))
        {
            throw new InvalidOperationException(string.Format(Template, themeName));
        }

        ThemeId = id;
    }

    /// <summary>
    /// Theme identifier.
    /// </summary>
    public string ThemeId { get; }

    /// <inheritdoc />
    public bool Equals(Theme? x, Theme? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.ThemeId == y.ThemeId;
    }

    /// <inheritdoc />
    public int GetHashCode(Theme obj)
    {
        return obj.ThemeId.GetHashCode();
    }
}