using Furaffinity.Client.Contracts;
using Furaffinity.Client.Exceptions;
using HtmlAgilityPack;

namespace Furaffinity.Client.Parsers;

/// <summary>
/// Checking for a possible exception after the site has sent a response.
/// </summary>
internal class ErrorParser
{
    /// <summary>
    /// Check page for generic exception that start with <h2>System Message</h2>
    /// </summary>
    /// <param name="page"></param>
    /// <exception cref="FuraffinityException"></exception>
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

        if (errorMessage?.Contains("flood") == true)
        {
            throw new FloodProtectionException();
        }
        
        switch (errorMessage)
        {
            case null:
                return;
            default:
                throw new FuraffinityException(errorMessage);
        }
    }

    /// <summary>
    /// Validate that submission exist and submission is found.
    /// </summary>
    /// <param name="page"></param>
    /// <exception cref="SubmissionNotFoundException"></exception>
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