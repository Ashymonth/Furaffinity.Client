using HtmlAgilityPack;

namespace Furaffinity.Client.Parsers;

internal class AccountAvatarParser
{
    public string GetAvatarLink(string page)
    {
        var document = new HtmlDocument();

        document.LoadHtml(page);

        var result = document.DocumentNode.Descendants("img")
            .FirstOrDefault(node => node.HasClass("user-nav-avatar"))
            ?.GetAttributeValue("src", string.Empty);

        return result ?? string.Empty;
    }
}