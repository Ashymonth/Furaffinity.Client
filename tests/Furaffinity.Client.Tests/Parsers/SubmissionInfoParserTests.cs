using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Furaffinity.Client.Constants;
using Furaffinity.Client.Contracts;
using Furaffinity.Client.Models;
using Furaffinity.Client.Parsers;
using Moq;
using Moq.Contrib.HttpClient;
using Xunit;

namespace Furaffinity.Client.Tests.Parsers;

public class SubmissionInfoParserTests
{
    [Fact]
    public async Task GetSubmissionAsyncTest_Should_Return_Submission()
    {
        const string fakeSubmissionFileUrl =
            "https://d.furaffinity.net/art/u15vier/1673727349/1673727349.u15vier_снег.jpg";

        var expectedUploadFile =
            new UploadFile(SubmissionTypeName.Artwork, "1673727349.u15vier_снег.jpg", new byte[] {1, 2, 3});

        var expected = Submission.CreateBuilder()
            .SetTitle("YCH (open)")
            .SetKeywords("ych snow blue")
            .SetRating("General")
            .SetCategory("ych / sale")
            .SetFile(expectedUploadFile)
            .SetFolderName("Open YCHs")
            .SetTheme("All")
            .SetSpecies("unspecified / any")
            .SetGender("Any")
            .PutInScraps(PutInScrap.Disabled())
            .Build();

        const string path = "TestData\\ImagePage.html";

        var page = await File.ReadAllTextAsync(path);

        var mock = new Mock<HttpMessageHandler>();
        mock.SetupRequest(HttpMethod.Get, fakeSubmissionFileUrl).ReturnsResponse(expectedUploadFile.Data);

        var parser = new SubmissionInfoParser(new DownloadClient(mock.CreateClient()));

        var actual = await parser.GetSubmissionAsync(page);

        Assert.Equivalent(expected, actual);
    }
}