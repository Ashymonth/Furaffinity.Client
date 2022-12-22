using System.IO;
using System.Threading.Tasks;
using Furaffinity.Client.Parsers;
using Xunit;

namespace Furaffinity.Client.Tests.Parsers;

public class SubmissionStatisticParserTest
{
    [Fact]
    public async Task GetStatisticTest_Should_Return_Submission_Statistic()
    {
        const int expectedViews = 150;
        const int expectedFavorites = 20;
        const int expectedComments = 33;
        const int expectedNumberOfComments = 33;
        
        const string path = "TestData\\FavedSubmission.html";

        var submissionPage = await File.ReadAllTextAsync(path);

        var parser = new SubmissionStatisticParser();

        var actual = parser.GetSubmissionStatistic(submissionPage);

        Assert.Equal(expectedViews, actual.Views);
        Assert.Equal(expectedFavorites, actual.Favorites);
        Assert.Equal(expectedComments, actual.Comments);
        Assert.Equal(expectedNumberOfComments, actual.UserComments.Count);
    }
}