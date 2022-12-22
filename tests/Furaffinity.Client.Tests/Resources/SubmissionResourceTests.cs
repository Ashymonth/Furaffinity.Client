using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
            Array.Empty<IFavAction>(), mockedClient);

        var actual = await submissionResource.GetCategoriesAsync(testsCookie);

        Assert.True(actual.Count == expectedThemeCount);
    }

    [Fact]
    public async Task FavSubmissionAsyncTest_Should_Fav_Submission()
    {
        const string testsCookie = "test";
        const string testSubmissionId = "50256309";
        const string testFavUrl = "fav/37238975/?key=9c2e30fdc83bbf0b9f855a97ecb2ec7f9cfc51ac";

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
        }, mockClient);

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
            mockedClient);
        
        var actual = await resource.GetSubmissionsStatisticAsync(new SubmissionId(testSubmissionId));
        
        Assert.Equal(expectedViews, actual.Views);
        Assert.Equal(expectedFavorites, actual.Favorites);
        Assert.Equal(expectedComments, actual.Comments);
        Assert.Equal(expectedNumberOfComments, actual.UserComments.Count);
        
    }
}