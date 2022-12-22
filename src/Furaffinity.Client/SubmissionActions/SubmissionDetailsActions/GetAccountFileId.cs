using Furaffinity.Client.Extensions;
using Furaffinity.Client.Parsers;
using Furaffinity.Client.RequestContext;

namespace Furaffinity.Client.SubmissionActions.SubmissionDetailsActions;

internal class GetAccountFileId : ISubmissionDetailsAction
{
    private readonly HttpClient _httpClient;

    public GetAccountFileId(HttpClient httpClient) =>
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

    public int Order => 2;

    public async Task ExecuteAsync(string cookie, SubmissionDetailsContext context, CancellationToken ct = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, context.AccountLink);
        request.AddCookie(cookie);
        
        using var response = await _httpClient.SendAsync(request, ct);

        var page = await response.Content.ReadAsStringAsync(ct);

        var parser = new AccountGalleryParser();

        var result = parser.GetSubmissionId(page);

        context.SubmissionId = result;
    }
}