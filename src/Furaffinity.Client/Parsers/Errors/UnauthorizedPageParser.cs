using Furaffinity.Client.Exceptions;
using HtmlAgilityPack;

namespace Furaffinity.Client.Parsers.Errors;

internal class UnauthorizedPageParser : IErrorParser
{
    private const string ErrorMessage = "System Message";
    
    public void ValidatePage(string page)
    {
        var document = new HtmlDocument();
        document.LoadHtml(page);

        var result = document
            .GetElementbyId("standardpage")
            ?.Descendants("section")
            .FirstOrDefault()
            ?.Descendants("div")
            .FirstOrDefault()
            ?.Descendants("h2")
            .FirstOrDefault()
            ?.InnerText;

        if (result == ErrorMessage)
        {
            throw new AuthorizationCookieException("Unable to authorize. Update or refresh cookie");
        }
    }
}