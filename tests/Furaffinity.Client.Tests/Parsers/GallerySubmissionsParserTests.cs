using System.IO;
using System.Linq;
using Furaffinity.Client.Models;
using Furaffinity.Client.Parsers;
using Xunit;

namespace Furaffinity.Client.Tests.Parsers;

public class GallerySubmissionsParserTests
{
    [Fact]
    public void GetGallerySubmissionLinksTest_Should_ReturnSubmission_Link()
    {
        const int expectedNumberOfLinks = 48;
        const string expectedStartWithLink = "/view/";
        
        const string path = "TestData\\GalleryPage.html";

        var page = File.ReadAllText(path);

        var parser = new GallerySubmissionsParser();

        var actual = parser.GetGallerySubmissionLinks(page);

        Assert.True(actual.Count == expectedNumberOfLinks);

        foreach (var submissionId in actual)
        {
            Assert.True(submissionId.Length == SubmissionId.MaxLength);
        }
    }
}