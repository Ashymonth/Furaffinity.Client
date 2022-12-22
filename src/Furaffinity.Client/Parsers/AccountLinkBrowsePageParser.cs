using Furaffinity.Client.Exceptions;
using HtmlAgilityPack;

namespace Furaffinity.Client.Parsers;

internal class AccountLinkBrowsePageParser
{
    public string GetAccountId(string page)
    {
        var document = new HtmlDocument();
        document.LoadHtml(page);

        var result = document.DocumentNode
            .Descendants("div")
            .FirstOrDefault(node => node.HasClass("aligncenter"))
            ?.Descendants("a")
            .FirstOrDefault()
            ?.GetAttributeValue("href", string.Empty);

        return result ?? throw new FuraffinityException("Unable to get account id");
    }
}