namespace Furaffinity.Client.Models;

/// <summary>
/// Submission category wrapper.
/// </summary>
public class Category
{
    private const string ExceptionTemplate = "Category: {0} not valid category";

    private static readonly Dictionary<string, string> NameToIdMap = new()
    {
        {"adoptables", "21"},
        {"all", "1"},
        {"artwork (digital)", "2"},
        {"artwork (traditional)", "3"},
        {"auctions", "22"},
        {"cellshading", "4"},
        {"contests", "23"},
        {"crafting", "5"},
        {"current events", "24"},
        {"designs", "6"},
        {"food / recipes", "32"},
        {"fursuiting", "8"},
        {"handhelds", "19"},
        {"icons", "9"},
        {"mosaics", "10"},
        {"music", "16"},
        {"other", "31"},
        {"photography", "11"},
        {"podcasts", "17"},
        {"poetry", "14"},
        {"prose", "15"},
        {"resources", "20"},
        {"scraps", "28"},
        {"screenshots", "27"},
        {"sculpting", "12"},
        {"skins", "18"},
        {"stockart", "26"},
        {"story", "13"},
        {"wallpaper", "29"},
        {"ych / sale", "30"},
    };

    internal Category(string? categoryName)
    {
        if (string.IsNullOrWhiteSpace(categoryName))
        {
            throw new ArgumentNullException(nameof(categoryName));
        }

        if (!NameToIdMap.TryGetValue(categoryName.ToLower(), out var id))
        {
            throw new InvalidOperationException(string.Format(ExceptionTemplate, categoryName));
        }

        CategoryId = id;
    }

    /// <summary>
    /// Category id.
    /// </summary>
    public string CategoryId { get; }
}