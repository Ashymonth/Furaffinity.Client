using Furaffinity.Client.Contracts;
using Furaffinity.Client.Exceptions;
using Furaffinity.Client.Extensions;
using Furaffinity.Client.Models;
using Furaffinity.Client.Parsers;
using Furaffinity.Client.RequestContext;
using Furaffinity.Client.SubmissionActions.SubmissionDeleteActions;
using Furaffinity.Client.SubmissionActions.SubmissionUploadActions;

namespace Furaffinity.Client.Resources;

/// <summary>
/// Access to action for account.
/// </summary>
public interface IAccountResource
{
    /// <summary>
    /// Return true if the auth cookie is 
    /// </summary>
    /// <param name="accountName">Account name.</param>
    /// <param name="cookie">Authentication cookie for account.</param>
    /// <param name="ct">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <exception cref="AuthorizationCookieException">If provided auth cookie for account are invalid</exception>
    /// <returns></returns>
    Task<bool> VerifyAccountAuthCookie(string accountName, string cookie, CancellationToken ct = default);

    /// <summary>
    /// Get account avatar data.
    /// </summary>
    /// <param name="cookie"></param>
    /// <param name="userName">Account name.</param>
    /// <param name="ct"> A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns></returns>
    Task<byte[]> GetAccountAvatarAsync(string cookie, string userName, CancellationToken ct = default);

    /// <summary>
    /// Upload submission to account.
    /// </summary>
    /// <param name="cookie">Account authentication cookie.</param>
    /// <param name="submission">Submission to upload.</param>
    /// <param name="ct"> A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns></returns>
    Task<string> UploadSubmissionAsync(string cookie,
        Submission submission,
        CancellationToken ct = default);

    /// <summary>
    /// Delete submission from account.
    /// </summary>
    /// <param name="submissionId">Submission identifier</param>
    /// <param name="cookie">Account authentication cookie.</param>
    /// <param name="password">Account password</param>
    /// <param name="ct"> A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns></returns>
    public Task<bool> DeleteSubmissionAsync(string submissionId,
        string cookie,
        string password,
        CancellationToken ct = default);
}

/// <summary>
/// <inheritdoc cref="IAccountResource"/>
/// </summary>
internal class AccountResource : IAccountResource
{
    private readonly HttpClient _cookieClient;
    private readonly HttpClient _downloadClient; // change to IDownloadClient;

    private readonly ISubmissionUploadAction[] _uploadActions;
    private readonly ISubmissionDeleteAction[] _deleteActions;

    public AccountResource(HttpClient requestClient,
        HttpClient downloadClient,
        ISubmissionUploadAction[] uploadActions,
        ISubmissionDeleteAction[] deleteActions)
    {
        _cookieClient = requestClient ?? throw new ArgumentNullException(nameof(requestClient));
        _downloadClient = downloadClient ?? throw new ArgumentNullException(nameof(downloadClient));
        _uploadActions = uploadActions ?? throw new ArgumentNullException(nameof(uploadActions));
        _deleteActions = deleteActions ?? throw new ArgumentNullException(nameof(deleteActions));
    }

    public async Task<bool> VerifyAccountAuthCookie(string accountName, string cookie, CancellationToken ct = default)
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"/stats/{accountName}/submissions/");
        requestMessage.AddCookie(cookie);

        using var responseMessage = await _cookieClient.SendAsync(requestMessage, ct);

        var page = await responseMessage.Content.ReadAsStringAsync(ct);
        try
        {
            ErrorParser.ValidatePage(page);

            return true;
        }
        catch (FuraffinityException)
        {
            return false;
        }
    }

    public async Task<byte[]> GetAccountAvatarAsync(string cookie, string accountName, CancellationToken ct = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/user/{accountName}");
        request.AddCookie(cookie);
        using var response = await _cookieClient.SendAsync(request, ct);

        var page = await response.Content.ReadAsStringAsync(ct);

        var parser = new AccountAvatarParser();

        var avatarLink = parser.GetAvatarLink(page);

        //unable to user GetByArrayAsync, because sometimes server return image with 404 status.
        using var avatarDownloadResponse = await _downloadClient.GetAsync($"https:{avatarLink}", ct);

        return await avatarDownloadResponse.Content.ReadAsByteArrayAsync(ct);
    }

    public async Task<string> UploadSubmissionAsync(string cookie,
        Submission submission,
        CancellationToken ct = default)
    {
        var context = new SubmissionUploadContext {Submission = submission};
        foreach (var uploadAction in _uploadActions.OrderBy(action => action.Order))
        {
            await uploadAction.ExecuteAsync(cookie, context, ct);
        }

        return context.SubmissionId;
    }

    public async Task<bool> DeleteSubmissionAsync(string submissionId,
        string cookie,
        string password,
        CancellationToken ct = default)
    {
        var context = new DeleteSubmissionContext
            {SubmissionId = new SubmissionId(submissionId), AccountPassword = password};

        foreach (var submissionDeleteAction in _deleteActions.OrderBy(action => action.Order))
        {
            await submissionDeleteAction.ExecuteAsync(cookie, context, ct);
        }

        return context.IsSuccessfully;
    }
}