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

        if (errorMessage is not null)
        {
            throw new FuraffinityException(errorMessage);
        }
    }
}