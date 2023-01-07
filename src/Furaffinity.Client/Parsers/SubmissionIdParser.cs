using Furaffinity.Client.Exceptions;
using HtmlAgilityPack;

namespace Furaffinity.Client.Parsers;

internal class SubmissionIdParser
{
    public string GetSubmissionId(string page)
    {
        var document = new HtmlDocument();
        document.LoadHtml(page);

        var result = document.DocumentNode
            .Descendants("form")
            .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty) == "postlink link-button")
            ?.Descendants("input")
            .FirstOrDefault()
            ?.GetAttributeValue("value", string.Empty);

        return result ?? throw new FuraffinityException("Unable to get submission id after upload submission");
    }
}