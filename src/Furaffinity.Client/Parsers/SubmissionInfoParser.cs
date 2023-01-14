using Furaffinity.Client.Contracts;
using Furaffinity.Client.Exceptions;
using Furaffinity.Client.Models;
using HtmlAgilityPack;

namespace Furaffinity.Client.Parsers;

internal class SubmissionInfoParser
{
    private readonly IDownloadClient _downloadClient;

    public SubmissionInfoParser(IDownloadClient downloadClient)
    {
        _downloadClient = downloadClient ?? throw new ArgumentNullException(nameof(downloadClient));
    }

    public async Task<Submission> GetSubmissionAsync(string page)
    {
        var document = new HtmlDocument();
        document.LoadHtml(page);

        var builder = Submission.CreateBuilder();

        var submissionParametersBlock = document.DocumentNode.Descendants("div");

        var sideBarBlock = submissionParametersBlock.FirstOrDefault(node => node.HasClass("submission-sidebar"));
        
        var submissionInfo = sideBarBlock
            ?.Descendants("section")
            .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty) == "info text");

        var categoryAndTheme = submissionInfo
            ?.Descendants("div")
            .FirstOrDefault()
            ?.Descendants("div")
            .FirstOrDefault();

        builder.SetTitle(ParseOrThrow(ParseTitle(document)));

        builder.SetRating(ParseOrThrow(ParseRating(document.DocumentNode)));

        builder.SetCategory(ParseOrThrow(ParseCategory(categoryAndTheme)));

        builder.SetTheme(ParseOrThrow(ParseTheme(categoryAndTheme)));

        builder.SetSpecies(ParseOrThrow(ParseSpecies(submissionInfo)));

        builder.SetGender(ParseOrThrow(ParseGender(submissionInfo)));

        builder.SetKeywords(ParseKeywords(sideBarBlock));

        builder.SetFolderName(ParseFolderName(sideBarBlock));

        builder.SetComments(ParseCommentStatus(document));

        var submissionUrl = ParseOrThrow(document.GetElementbyId("submissionImg")
            ?.GetAttributeValue("data-fullview-src", string.Empty));

        var submissionFile = await _downloadClient.DownloadSubmissionFile(submissionUrl);

        builder.SetFile(submissionFile);

        return builder.Build();
    }

    private static string? ParseRating(HtmlNode? htmlNode)
    {
        var rating = htmlNode
            ?.Descendants("div")
            .FirstOrDefault(node => node.HasClass("rating"))
            ?.Descendants("span")
            .FirstOrDefault()
            ?.InnerText
            ?.Trim();

        return rating;
    }

    private static string? ParseFolderName(HtmlNode? sideBarBlock)
    {
        var folderName = sideBarBlock?
            .Descendants("section")
            .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty) == "folder-list-container text")
            ?.Descendants("div")
            .FirstOrDefault()
            ?.Descendants("a")
            .FirstOrDefault()
            ?.Descendants("span")
            ?.FirstOrDefault()
            ?.InnerText;

        return folderName;
    }

    private static string? ParseKeywords(HtmlNode? sideBarBlock)
    {
        var keywordsList = sideBarBlock?.Descendants("section")
            .FirstOrDefault(node => node.HasClass("tags-row"))
            ?.Descendants("span")
            .Select(node =>
            {
                var innerText = node.Descendants("a").FirstOrDefault()?.InnerText;
                return innerText ?? string.Empty;
            });

        var keywords = keywordsList != null
            ? string.Join(" ", keywordsList)
            : null;

        return keywords;
    }

    private static string? ParseGender(HtmlNode? submissionInfo)
    {
        var gender = submissionInfo
            ?.Descendants("div")
            .Skip(3) // block with gender
            .FirstOrDefault()
            ?.Descendants("span")
            .FirstOrDefault()
            ?.InnerText;

        return gender;
    }

    private static string? ParseSpecies(HtmlNode? submissionInfo)
    {
        var species = submissionInfo
            ?.Descendants("div")
            .Skip(2) // block with species
            .FirstOrDefault()
            ?.Descendants("span")
            .FirstOrDefault()
            ?.InnerText;

        return species;
    }

    private static string? ParseTheme(HtmlNode? categoryAndTheme)
    {
        var theme = categoryAndTheme
            ?.Descendants("span")
            .FirstOrDefault(node => node.HasClass("type-name"))
            ?.InnerText;

        return theme;
    }

    private static string? ParseCategory(HtmlNode? categoryAndTheme)
    {
        var category = categoryAndTheme
            ?.Descendants("span")
            .FirstOrDefault(node => node.HasClass("category-name"))
            ?.InnerText;

        return category;
    }

    private static string? ParseTitle(HtmlDocument document)
    {
        var title = document.DocumentNode.Descendants("div")
            .FirstOrDefault(node => node.HasClass("submission-title"))
            ?.Descendants("h2")
            .FirstOrDefault()
            ?.Descendants("p")
            .FirstOrDefault()
            ?.InnerText;

        return title;
    }

    private static Comments ParseCommentStatus(HtmlDocument document)
    {
        var result = document.GetElementbyId("responsebox")
            ?.Descendants("form") //  if any form element exist then comments enabled otherwise comments disabled
            .Any();

        return result is true ? Comments.Enabled() : Comments.Disabled();
    }

    private static string ParseOrThrow(string? result)
    {
        if (result is null)
        {
            throw new FuraffinityException("Unable to parse submission");
        }

        return result;
    }
}