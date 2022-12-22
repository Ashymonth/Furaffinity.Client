using Furaffinity.Client.Extensions;
using Furaffinity.Client.Parsers;
using Furaffinity.Client.RequestContext;

namespace Furaffinity.Client.SubmissionActions.SubmissionDetailsActions;

internal class GetAccountLinkId : ISubmissionDetailsAction
{
    private readonly HttpClient _httpClient;

    public GetAccountLinkId(HttpClient httpClient) =>
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

    public int Order => 1;

    public async Task ExecuteAsync(string cookie, SubmissionDetailsContext context, CancellationToken ct)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "/browse");
        request.AddCookie(cookie);

        using var response = await _httpClient.SendAsync(request, ct);

        var page = await response.Content.ReadAsStringAsync(ct);

        var parser = new AccountLinkBrowsePageParser();

        var result = parser.GetAccountId(page);

        context.AccountLink = result;
    }
}