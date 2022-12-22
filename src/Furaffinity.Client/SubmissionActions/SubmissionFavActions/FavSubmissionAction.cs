using Furaffinity.Client.Extensions;
using Furaffinity.Client.Parsers;
using Furaffinity.Client.RequestContext;

namespace Furaffinity.Client.SubmissionActions.SubmissionFavActions;

/// <summary>
/// Last step on fav action.
/// </summary>
internal class FavSubmissionAction : IFavAction
{
    private readonly HttpClient _httpClient;

    public FavSubmissionAction(HttpClient httpClient) =>
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

    public int Order => 2;

    public async Task ExecuteAsync(string cookie, FavContext context, CancellationToken ct = default)
    {
        using var favMessage = new HttpRequestMessage(HttpMethod.Get, context.FavUrl);
        favMessage.AddCookie(cookie);

        using var favResponse = await _httpClient.SendAsync(favMessage, ct);

        var favContent = await favResponse.Content.ReadAsStringAsync(ct);

        var parser = new FavLinkParser();

        var unFavLink = parser.GetUnFavLink(favContent);

        // ensure, that after we send fav request image has unFav button and all is good.
        context.IsSucceed = !string.IsNullOrWhiteSpace(unFavLink);
    }
}