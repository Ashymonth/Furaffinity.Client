using HtmlAgilityPack;

namespace Furaffinity.Client.Parsers;

internal class FavLinkParser
{
    private const string FavLinkBegin = "/fav/";
    private const string UnFavLinkBegin = "/unfav/";

    public string GetFavLink(string page)
    {
        var result = GetLink(page);

        return result.StartsWith(FavLinkBegin) ? result : string.Empty;
    }

    public string GetUnFavLink(string page)
    {
        var result = GetLink(page);

        return result.StartsWith(UnFavLinkBegin) ? result : string.Empty;
    }

    private static string GetLink(string page)
    {
        var document = new HtmlDocument();
        document.LoadHtml(page);

        var result = document.GetElementbyId("columnpage")
            .Descendants("section")
            .FirstOrDefault(node => node.HasClass("buttons"))
            ?.Descendants("div")
            .FirstOrDefault(node => node.HasClass("fav"))
            ?.Descendants("a")
            .FirstOrDefault()
            ?.GetAttributeValue("href", string.Empty);

        return result ?? string.Empty;
    }
}