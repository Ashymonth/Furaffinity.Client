using Furaffinity.Client.Extensions;
using Furaffinity.Client.Parsers;
using Furaffinity.Client.RequestContext;

namespace Furaffinity.Client.SubmissionActions.SubmissionDetailsActions;

internal class GetSubmissionDetails : ISubmissionDetailsAction
{
    private readonly HttpClient _httpClient;

    public GetSubmissionDetails(HttpClient httpClient) =>
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

    public int Order => 3;

    public async Task ExecuteAsync(string cookie, SubmissionDetailsContext context, CancellationToken ct = default)
    {
        using var request =
            new HttpRequestMessage(HttpMethod.Get, $"/controls/submissions/changeinfo/{context.SubmissionId}");
        request.AddCookie(cookie);

        using var response = await _httpClient.SendAsync(request, ct);

        var page = await response.Content.ReadAsStringAsync(ct);
        var parser = new CategoryParser();

        var result = parser.GetCategories(page);

        context.SubmissionsDetails = result;
    }
}