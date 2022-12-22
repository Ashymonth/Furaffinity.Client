using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Furaffinity.Client.Models;
using Furaffinity.Client.Resources;
using Furaffinity.Client.Tests.Extensions;
using Moq;
using Moq.Contrib.HttpClient;
using Xunit;

namespace Furaffinity.Client.Tests.Resources;

public class GalleryResourcesTests
{
    [Fact]
    public async Task GetGallerySubmissionLinksTest_Should_Return_Links()
    {
        const string testsCookie = "test";
        
        const int expectedNumberOfLinks = 48;
        const string expectedStartWithLink = "/view/";

        var fakeGalleryResponsePage = await File.ReadAllTextAsync("TestData\\GalleryPage.html");

        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.SetupRequest(HttpMethod.Get, $"{Constants.BaseUrl}/browse")
            .ReturnsResponse(HttpStatusCode.OK, new StringContent(fakeGalleryResponsePage));
        
        var mockedClient = mockHandler.CreateClientWithBaseUrl();

        var galleryResource = new GalleryResource(mockedClient);

        var actual = await galleryResource.GetGallerySubmissionLinks(testsCookie);

        Assert.True(actual.Count == expectedNumberOfLinks);
        foreach (var submissionId in actual)
        {
            Assert.True(submissionId.Length == SubmissionId.MaxLength);
        }
    }
}