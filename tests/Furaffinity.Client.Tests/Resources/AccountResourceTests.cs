using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Furaffinity.Client.Constants;
using Furaffinity.Client.Contracts;
using Furaffinity.Client.Models;
using Furaffinity.Client.Requests;
using Furaffinity.Client.Resources;
using Furaffinity.Client.SubmissionActions.SubmissionDeleteActions;
using Furaffinity.Client.SubmissionActions.SubmissionUploadActions;
using Furaffinity.Client.Tests.Extensions;
using Moq;
using Moq.Contrib.HttpClient;
using Xunit;

namespace Furaffinity.Client.Tests.Resources;

public class AccountResourceTests
{
    [Fact]
    public async Task GetAvatarDataTest_Should_Return_Avatar_Data()
    {
        const string userName = "TestAccount";
        const string parsedLinkToAccountAvatar = "https://a.furaffinity.net/20221112/sweetyfoxy656.gif";

        var expected = new byte[] {1, 2, 3};

        var fakeUserPageResponse = await File.ReadAllTextAsync("TestData\\AccountGalleryPage.html");

        var mockHandler = new Mock<HttpMessageHandler>();

        mockHandler.SetupRequest(HttpMethod.Get, $"{Constants.BaseUrl}/user/{userName}")
            .ReturnsResponse(HttpStatusCode.OK, new StringContent(fakeUserPageResponse));

        mockHandler.SetupRequest(HttpMethod.Get, parsedLinkToAccountAvatar)
            .ReturnsResponse(HttpStatusCode.OK, new ByteArrayContent(new byte[] {1, 2, 3}));
        
        var requestMockClient = mockHandler.CreateClientWithBaseUrl();

        var downloadMockClient = mockHandler.CreateClient();

        var accountResource =
            new AccountResource(requestMockClient, downloadMockClient, Array.Empty<ISubmissionUploadAction>(),
                Array.Empty<ISubmissionDeleteAction>());

        var actual = await accountResource.GetAccountAvatarAsync(userName);

        Assert.Equal(actual, expected);
    }

    [Fact]
    public async Task UploadSubmissionTest_Should_Upload_Submission()
    {
        const string expectedSubmissionId = "37238975";
        const string testCookie = "cookie";
        const string testFormKey = "ff72a266ce57b1cc4aae8dc8e2941e4524284a88";

        var fakeUploadPage = await File.ReadAllTextAsync("TestData\\UploadSubmissionPage.html");
        var fakeFinalizePage = await File.ReadAllTextAsync("TestData\\FinalizeSubmissionPage.html");
        var fakeSubmissionPage = await File.ReadAllTextAsync("TestData\\ImagePage.html");

        var expectedSubmission = Submission.CreateBuilder()
            .SetTitle("test title")
            .SetDescription("test description")
            .SetKeywords("tesst keywords")
            .SetFile(SubmissionTypeName.Artwork, "test submission.jpg", new byte[] {1, 2, 3})
            .SetRating("general")
            .SetCategory("All")
            .SetTheme("All")
            .SetSpecies("Akita")
            .SetGender("Any")
            .SetFolderName("test folder")
            .PutInScraps(PutInScrap.Disabled())
            .Build();

        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.SetupRequest(HttpMethod.Get, $"{Constants.BaseUrl}/submit/")
            .ReturnsResponse(HttpStatusCode.OK, new StringContent(fakeUploadPage));

        mockHandler.SetupRequest(HttpMethod.Post, $"{Constants.BaseUrl}/submit/upload/", async message =>
            {
                using var testRequest = new UploadSubmissionsRequest(testCookie, testFormKey, expectedSubmission.File);

                return await message.CompareRequestsAsync(testRequest.RequestMessage);
            })
            .ReturnsResponse(HttpStatusCode.OK, new StringContent(fakeFinalizePage));

        mockHandler.SetupRequest(HttpMethod.Post, $"{Constants.BaseUrl}/submit/finalize/", async message =>
            {
                using var testRequest = new FinalizeSubmissionUploadRequest(testCookie, testFormKey, expectedSubmission);

                return await message.CompareRequestsAsync(testRequest.RequestMessage);
            })
            .ReturnsResponse(HttpStatusCode.OK, new StringContent(fakeSubmissionPage));

        var mockClient = mockHandler.CreateClientWithBaseUrl();

        var accountResource = new AccountResource(mockClient, mockClient, new ISubmissionUploadAction[]
        {
            new GetInitialFormKeyAction(mockClient),
            new UploadSubmissionAction(mockClient),
            new FinalizeSubmissionsAction(mockClient)
        }, Array.Empty<ISubmissionDeleteAction>());

        var actual = await accountResource.UploadSubmissionAsync(testCookie, expectedSubmission);

        Assert.Equal(expectedSubmissionId, actual);
    }
    
    [Fact]
    public async Task DeleteSubmissionTest_Should_Delete_Submission()
    {
        const string testSubmissionId = "12345678";
        const string testCookie = "cookie";
        const string testPassword = "123456";
        const string testFormKey = "2c5b025c027b46b43e419d205f60fd5b4c29968c";

        var fakeConfirmDeletePage = await File.ReadAllTextAsync("TestData\\ConfirmDeletePage.html");
        var fakeAccountPage = await File.ReadAllTextAsync("TestData\\AccountSubmisssionsManagePage.html");

        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler
            .SetupRequest(HttpMethod.Post, $"{Constants.BaseUrl}/controls/submissions/", async message =>
            {
                using var originalRequest = new DeleteSubmissionRequest(new SubmissionId(testSubmissionId), testCookie);

                return await message.CompareRequestsAsync(originalRequest.RequestMessage);
            })
            .ReturnsResponse(HttpStatusCode.OK, new StringContent(fakeConfirmDeletePage));

        mockHandler.SetupRequest(HttpMethod.Post, $"{Constants.BaseUrl}/controls/submissions/", async message =>
            {
                using var originalRequest = new ConfirmDeleteSubmissionRequest(testCookie, testFormKey, testPassword,
                    new SubmissionId(testSubmissionId));

                return await message.CompareRequestsAsync(originalRequest.RequestMessage);
            })
            .ReturnsResponse(HttpStatusCode.OK, new StringContent(fakeAccountPage));

        var mockClient = mockHandler.CreateClientWithBaseUrl();

        var resource = new AccountResource(mockClient, mockClient, Array.Empty<ISubmissionUploadAction>(),
            new ISubmissionDeleteAction[]
            {
                new DeleteSubmissionAction(mockClient),
                new ConfirmDeleteSubmissionAction(mockClient)
            });

        var actual = await resource.DeleteSubmissionAsync(testSubmissionId, testCookie, testPassword);

        Assert.True(actual);
    }
}