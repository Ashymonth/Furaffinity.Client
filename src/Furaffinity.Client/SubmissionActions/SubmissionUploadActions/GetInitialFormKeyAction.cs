using Furaffinity.Client.Extensions;
using Furaffinity.Client.Parsers;
using Furaffinity.Client.RequestContext;

namespace Furaffinity.Client.SubmissionActions.SubmissionUploadActions;

/// <summary>
/// Initial action to obtain form key from upload page.
/// </summary>
internal class GetInitialFormKeyAction : ISubmissionUploadAction
{
    private readonly HttpClient _httpClient;

    public GetInitialFormKeyAction(HttpClient httpClient) => _httpClient = httpClient;

    public int Order => 1;

    public async Task ExecuteAsync(string cookie, SubmissionUploadContext context, CancellationToken ct)
    {
        using var initialRequest = new HttpRequestMessage(HttpMethod.Get, "/submit/");
        initialRequest.AddCookie(cookie);

        using var response = await _httpClient.SendAsync(initialRequest, ct);

        var submissionPage = await response.Content.ReadAsStringAsync(ct);
        
        var parser = new FormKeyParser();

        context.FormKey = parser.GetFormKey(submissionPage);
    }

  
}