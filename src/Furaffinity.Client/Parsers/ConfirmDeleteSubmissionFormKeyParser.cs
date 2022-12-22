using HtmlAgilityPack;

namespace Furaffinity.Client.Parsers;

internal class ConfirmDeleteSubmissionFormKeyParser
{
    public string GetFormKey(string page)
    {
        ErrorParser.ValidatePage(page);
        
        var document = new HtmlDocument();
        document.LoadHtml(page);

        var result = document.DocumentNode
            .Descendants("button")
            .FirstOrDefault(node => node.GetAttributeValue("name", string.Empty) == "confirm")
            ?.GetAttributeValue("value", string.Empty);

        return result ?? string.Empty;
    }
}