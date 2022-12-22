using HtmlAgilityPack;

namespace Furaffinity.Client.Parsers;

internal class AccountManageSubmissionParser
{
    public bool HasAnySubmissions(string page)
    {
        var document = new HtmlDocument();
        document.LoadHtml(page);

        var result = document.GetElementbyId("gallery-manage-submissions")
            ?.Descendants("figure")
            .Any();

        return result == true;
    }
}