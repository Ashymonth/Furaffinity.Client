using Furaffinity.Client.Extensions;
using Furaffinity.Client.Parsers;
using Furaffinity.Client.RequestContext;

namespace Furaffinity.Client.SubmissionActions.SubmissionFavActions;

/// <summary>
/// First action to fav image
/// </summary>
internal class GetFavLink : IFavAction
{
    private readonly HttpClient _httpClient;

    public GetFavLink(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public int Order => 1;

    public async Task ExecuteAsync(string cookie, FavContext context, CancellationToken ct = default)
    {
        using var message = new HttpRequestMessage(HttpMethod.Get, context.SubmissionUrl);
        message.AddCookie(cookie);

        // we go to the submission page and look for fav link. example /fav/123213/?key=123123
        using var response = await _httpClient.SendAsync(message, ct);

        var page = await response.Content.ReadAsStringAsync(ct);

        var parser = new FavLinkParser();

        var favLink = parser.GetFavLink(page);

        context.FavUrl = favLink;
    }
}