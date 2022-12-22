using Furaffinity.Client.Parsers;
using Furaffinity.Client.RequestContext;
using Furaffinity.Client.Requests;

namespace Furaffinity.Client.SubmissionActions.SubmissionUploadActions;

/// <summary>
/// Final action to upload submission on site.
/// </summary>
internal class FinalizeSubmissionsAction : ISubmissionUploadAction
{
    private readonly HttpClient _httpClient;

    public FinalizeSubmissionsAction(HttpClient httpClient) =>
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

    public int Order => 3;

    public async Task ExecuteAsync(string cookie, SubmissionUploadContext context, CancellationToken ct)
    {
        using var request = new FinalizeSubmissionUploadRequest(cookie, context.FormKey, context.Submission);
        
        using var response = await _httpClient.SendAsync(request.RequestMessage, ct);

        var imagePage = await response.Content.ReadAsStringAsync(ct);

        var parser = new SubmissionIdParser();

        //ensure, that submission uploaded and we redirected to it. 
        context.SubmissionId = parser.GetSubmissionId(imagePage);
    }
}