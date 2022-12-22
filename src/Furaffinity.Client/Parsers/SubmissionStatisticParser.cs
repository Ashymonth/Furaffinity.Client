using Furaffinity.Client.Contracts;
using Furaffinity.Client.Models;
using HtmlAgilityPack;

namespace Furaffinity.Client.Parsers;

internal class SubmissionStatisticParser
{
    public SubmissionStatistic GetSubmissionStatistic(string submissionPage)
    {
        var document = new HtmlDocument();
        document.LoadHtml(submissionPage);

        var statisticContainer = document.GetElementbyId("columnpage")
            ?.Descendants("section")
            .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty) == "stats-container text") // HasClass not work
            ?.Descendants("div")
            .ToArray();

        var submissionStatistic = new SubmissionStatistic
        {
            Views = GetStatisticValue("views", statisticContainer),
            Comments = GetStatisticValue("comments", statisticContainer),
            Favorites = GetStatisticValue("favorites", statisticContainer),
        };

        var commentsContainer = document.GetElementbyId("comments-submission")
            ?.Descendants("div")
            .Where(node => node.HasClass("comment_container"));

        if (commentsContainer is null)
        {
            return new SubmissionStatistic();
        }

        foreach (var commentContainer in commentsContainer)
        {
            var commentBlock = commentContainer
                .Descendants("div")
                .FirstOrDefault(node => node.HasClass("comment-content"));

            if (commentBlock is null)
            {
                continue;
            }

            var authorName = commentBlock
                .Descendants("comment-username")
                .FirstOrDefault()
                ?.Descendants("a")
                .FirstOrDefault()
                ?.Descendants("strong")
                .FirstOrDefault()
                ?.InnerText;

            var text = commentBlock
                .Descendants("comment-user-text")
                .FirstOrDefault()
                ?.Descendants("div")
                ?.FirstOrDefault()
                ?.InnerText.Trim();

            var userComment = new UserComment(authorName, text);

            submissionStatistic.AddUserComment(userComment);
        }

        return submissionStatistic;
    }

    private static int GetStatisticValue(string className, HtmlNode[]? statisticContainer)
    {
        var statisticValue = statisticContainer?
            .FirstOrDefault(node => node.HasClass(className))
            ?.Descendants("span")
            .FirstOrDefault()
            ?.InnerText;

        return int.TryParse(statisticValue, out var value) ? value : 0;
    }
}