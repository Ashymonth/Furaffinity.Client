using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Furaffinity.Client.Constants;
using Furaffinity.Client.Contracts;
using Furaffinity.Client.Exceptions;
using Furaffinity.Client.Models;
using Furaffinity.Client.Resources;
using Furaffinity.Client.SubmissionActions.SubmissionDetailsActions;
using Furaffinity.Client.SubmissionActions.SubmissionFavActions;
using Furaffinity.Client.Tests.Extensions;
using Moq;
using Moq.Contrib.HttpClient;
using Xunit;

namespace Furaffinity.Client.Tests.Resources;

public class SubmissionResourceTests
{
    [Fact]
    public async Task GetCategoriesAsyncTest_Should_Return_Categories()
    {
        const int expectedThemeCount = 4;

        const string testsCookie = "test";
        const string testAccountUserLink = "user/sweetyfoxy656/";
        const string testSubmissionId = "37238975";

        var fakeAccountPageResponse = await File.ReadAllTextAsync("TestData\\CategoryPage.html");
        var fakeAccountGalleryResponse = await File.ReadAllTextAsync("TestData\\AccountGalleryPage.html");
        var fakeSubmissionPageResponse = await File.ReadAllTextAsync("TestData\\CategoryPage.html");

        var mock = new Mock<HttpMessageHandler>();

        mock.SetupRequest(HttpMethod.Get, $"{Constants.BaseUrl}/browse")
            .ReturnsResponse(HttpStatusCode.OK, new StringContent(fakeAccountPageResponse));

        mock.SetupRequest(HttpMethod.Get, $"{Constants.BaseUrl}/{testAccountUserLink}")
            .ReturnsResponse(HttpStatusCode.OK, new StringContent(fakeAccountGalleryResponse));

        mock.SetupRequest(HttpMethod.Get, $"{Constants.BaseUrl}/controls/submissions/changeinfo/{testSubmissionId}")
            .ReturnsResponse(HttpStatusCode.OK, new StringContent(fakeSubmissionPageResponse));

        var mockedClient = mock.CreateClientWithBaseUrl();

        var actions = new ISubmissionDetailsAction[]
        {
            new GetSubmissionDetails(mockedClient),
            new GetAccountFileId(mockedClient),
            new GetAccountLinkId(mockedClient)
        };

        var submissionResource = new SubmissionResource(actions.OrderBy(action => action.Order).ToArray(),
            Array.Empty<IFavAction>(), new DownloadClient(mockedClient), mockedClient);

        var actual = await submissionResource.GetCategoriesAsync(testsCookie);

        Assert.True(actual.Count == expectedThemeCount);
    }

    [Fact]
    public async Task FavSubmissionAsyncTest_Should_Fav_Submission()
    {
        const string testsCookie = "test";
        const string testSubmissionId = "50608865";
        const string testFavUrl = "fav/50608865/?key=7a1e52fd72fdf54776c749bf47895a38b454b2c7";

        var fakeSubmissionPage = await File.ReadAllTextAsync("TestData\\ImagePage.html");
        var fakeFavedSubmissionPage = await File.ReadAllTextAsync("TestData\\FavedSubmission.html");

        var mock = new Mock<HttpMessageHandler>();

        mock.SetupRequest(HttpMethod.Get, $"{Constants.BaseUrl}/view/{testSubmissionId}/")
            .ReturnsResponse(HttpStatusCode.OK, new StringContent(fakeSubmissionPage));

        mock.SetupRequest(HttpMethod.Get, $"{Constants.BaseUrl}/{testFavUrl}")
            .ReturnsResponse(HttpStatusCode.OK, new StringContent(fakeFavedSubmissionPage));

        var mockClient = mock.CreateClientWithBaseUrl();

        var submissionResource = new SubmissionResource(Array.Empty<ISubmissionDetailsAction>(), new IFavAction[]
        {
            new GetFavLink(mockClient),
            new FavSubmissionAction(mockClient)
        }, new DownloadClient(mockClient), mockClient);

        var actual = await submissionResource.FavSubmissionAsync(testsCookie, new SubmissionId(testSubmissionId));

        Assert.True(actual);
    }

    [Fact]
    public async Task GetSubmissionStatisticTest_Should_Return_Submission_Statistic()
    {
        const int expectedViews = 150;
        const int expectedFavorites = 20;
        const int expectedComments = 33;
        const int expectedNumberOfComments = 33;

        const string testSubmissionId = "12345";
        const string testSubmissionUrl = $"view/{testSubmissionId}";

        var testSubmissionPage = await File.ReadAllTextAsync("TestData\\FavedSubmission.html");

        var mock = new Mock<HttpMessageHandler>();

        mock.SetupRequest(HttpMethod.Get, $"{Constants.BaseUrl}/{testSubmissionUrl}/")
            .ReturnsResponse(HttpStatusCode.OK, new StringContent(testSubmissionPage));

        var mockedClient = mock.CreateClientWithBaseUrl();

        var resource = new SubmissionResource(Array.Empty<ISubmissionDetailsAction>(), Array.Empty<IFavAction>(),
            new DownloadClient(mockedClient), mockedClient);

        var actual = await resource.GetSubmissionsStatisticAsync(new SubmissionId(testSubmissionId));

        Assert.Equal(expectedViews, actual.Views);
        Assert.Equal(expectedFavorites, actual.Favorites);
        Assert.Equal(expectedComments, actual.Comments);
        Assert.Equal(expectedNumberOfComments, actual.UserComments.Count);
    }

    [Fact]
    public async Task GetSubmissionTest_Should_Return_Submission()
    {
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
        
        const string fakeSubmissionFileUrl =
            "https://d.furaffinity.net/art/u15vier/1673727349/1673727349.u15vier_снег.jpg";
        
        const string fakeSubmissionId = "12345678";
        
        var fakeSubmissionPage = await File.ReadAllTextAsync("TestData\\ImagePage.html");
        
        var primaryClientMock = new Mock<HttpMessageHandler>();

        primaryClientMock.SetupRequest(HttpMethod.Get, $"{Constants.BaseUrl}/view/{fakeSubmissionId}/")
            .ReturnsResponse(HttpStatusCode.OK, new StringContent(fakeSubmissionPage));

        var downloadClientMock = new Mock<HttpMessageHandler>();
        
        downloadClientMock.SetupRequest(HttpMethod.Get, fakeSubmissionFileUrl)
            .ReturnsResponse(expectedUploadFile.Data);
        
        var mockClient = primaryClientMock.CreateClientWithBaseUrl();

        var submissionResource = new SubmissionResource(Array.Empty<ISubmissionDetailsAction>(), new IFavAction[]
        {
            new GetFavLink(mockClient),
            new FavSubmissionAction(mockClient)
        }, new DownloadClient(downloadClientMock.CreateClient()), mockClient);

        var actual = await submissionResource.GetSubmissionById(new SubmissionId(fakeSubmissionId));

        Assert.Equivalent(expected, actual);
    }

    [Fact]
    public async Task GetSubmissionTest_Should_Throw_Submission_Not_Found_Exception()
    {
        const string fakeSubmissionId = "12345678";
        
        var fakeSubmissionNotFoundPage = await File.ReadAllTextAsync("TestData\\SubmissionNotFoundPage.html");

        var primaryClientMock = new Mock<HttpMessageHandler>();

        primaryClientMock.SetupRequest(HttpMethod.Get, $"{Constants.BaseUrl}/view/{fakeSubmissionId}/")
            .ReturnsResponse(HttpStatusCode.OK, new StringContent(fakeSubmissionNotFoundPage));
        
        var mockClient = primaryClientMock.CreateClientWithBaseUrl();

        var submissionResource = new SubmissionResource(Array.Empty<ISubmissionDetailsAction>(), new IFavAction[]
        {
            new GetFavLink(mockClient),
            new FavSubmissionAction(mockClient)
        }, new DownloadClient(mockClient), mockClient);

        await Assert.ThrowsAsync<SubmissionNotFoundException>(() =>
            submissionResource.GetSubmissionById(new SubmissionId(fakeSubmissionId)));

    }
}