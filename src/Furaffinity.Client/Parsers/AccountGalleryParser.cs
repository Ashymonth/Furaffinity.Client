using HtmlAgilityPack;

namespace Furaffinity.Client.Parsers;

internal class AccountGalleryParser
{
    public string GetSubmissionId(string page)
    {
        var document = new HtmlDocument();
        document.LoadHtml(page);

        var result = document.GetElementbyId("gallery-latest-submissions")
            .Descendants("figure")
            .FirstOrDefault()
            ?.Descendants("b")
            .FirstOrDefault()
            ?.Descendants("u")
            .FirstOrDefault()
            ?.Descendants("a")
            .FirstOrDefault()
            ?.GetAttributeValue("href", string.Empty);

        return result is not null ? result.Split('/')[2] : string.Empty;
    }
}