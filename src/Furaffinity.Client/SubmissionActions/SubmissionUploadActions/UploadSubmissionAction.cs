using Furaffinity.Client.Parsers;
using Furaffinity.Client.RequestContext;
using Furaffinity.Client.Requests;

namespace Furaffinity.Client.SubmissionActions.SubmissionUploadActions;

/// <summary>
/// First step to upload submission.
/// </summary>
internal class UploadSubmissionAction : ISubmissionUploadAction
{
    private readonly HttpClient _httpClient;

    public UploadSubmissionAction(HttpClient httpClient) =>
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

    public int Order => 2;

    public async Task ExecuteAsync(string cookie, SubmissionUploadContext context, CancellationToken ct)
    {
        using var uploadRequest = new UploadSubmissionsRequest(cookie, context.FormKey, context.Submission.File);

        using var uploadResponse = await _httpClient.SendAsync(uploadRequest.RequestMessage, ct);

        var finalizePage = await uploadResponse.Content.ReadAsStringAsync(ct);

        context.FormKey = new FormKeyParser().GetFormKey(finalizePage);
    }
}