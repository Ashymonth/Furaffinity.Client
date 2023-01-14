using Furaffinity.Client.Contracts;
using Furaffinity.Client.Exceptions;
using HtmlAgilityPack;

namespace Furaffinity.Client.Parsers;

internal class ErrorParser
{
    public static void ValidatePage(string page)
    {
        var document = new HtmlDocument();
        document.LoadHtml(page);

        var errorMessage = document
            .GetElementbyId("standardpage")
            ?.Descendants("section")
            .FirstOrDefault()
            ?.Descendants("div")
            .FirstOrDefault()
            ?.Descendants("div")
            ?.FirstOrDefault()
            ?.InnerText;

        switch (errorMessage)
        {
            case null:
                return;
            default:
                throw new FuraffinityException(errorMessage);
        }
    }

    public static void ValidateSubmissionPage(string page)
    {
        var document = new HtmlDocument();
        document.LoadHtml(page);

        var result = document.DocumentNode
            .Descendants("div")
            .FirstOrDefault(node => node.HasClass("section-body"))
            ?.InnerText;

        if (result?.Trim().Contains(ExceptionText.SubmissionNotFoundMessage) == true)
        {
            throw new SubmissionNotFoundException(result);
        }
    }
}