using HtmlAgilityPack;

namespace Furaffinity.Client.Parsers;

internal class SubmissionIdParser
{
    public string GetSubmissionId(string page)
    {
        var document = new HtmlDocument();
        document.LoadHtml(page);

        var result = document.GetElementbyId("responsebox")
            .Descendants("form")
            .FirstOrDefault()
            ?.GetAttributeValue("action", string.Empty)
            .Split("/", StringSplitOptions.RemoveEmptyEntries)
            .LastOrDefault();

        return result ?? string.Empty;
    }
}