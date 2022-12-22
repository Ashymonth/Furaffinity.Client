using Furaffinity.Client.Exceptions;
using Furaffinity.Client.Models;
using HtmlAgilityPack;

namespace Furaffinity.Client.Parsers;

internal class GallerySubmissionsParser
{
    public List<string> GetGallerySubmissionLinks(string page)
    {
        var document = new HtmlDocument();
        document.LoadHtml(page);

        var submissionSection = document.GetElementbyId("gallery-browse").Descendants("figure");

        var result = new List<string>();
        foreach (var submissionNode in submissionSection)
        {
            var submissionLink = submissionNode
                .Descendants("b")
                .FirstOrDefault()
                ?.Descendants("u")
                .FirstOrDefault()
                ?.Descendants("a")
                .FirstOrDefault()
                ?.GetAttributeValue("href", string.Empty);

            // /view/12345678/ - start from last submission id number 8 (length - 1)
            var submissionId = submissionLink?.Substring(submissionLink.Length - 1 - SubmissionId.MaxLength,
                SubmissionId.MaxLength) ?? throw new FuraffinityException("Unable to get submission id");

            result.Add(submissionId);
        }

        return result;
    }
}