using Furaffinity.Client.Extensions;
using Furaffinity.Client.Parsers;

namespace Furaffinity.Client.Resources;

/// <summary>
/// Access to action with gallery page.
/// </summary>
public interface IGalleryResource
{
    /// <summary>
    /// Get submission ids from gallery page.
    /// </summary>
    /// <param name="cookie">Account authentication cookie.</param>
    /// <param name="ct"> A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns></returns>
    Task<IList<string>> GetGallerySubmissionLinks(string cookie, CancellationToken ct = default);
}

/// <summary>
/// <inheritdoc cref="IGalleryResource"/>
/// </summary>
internal class GalleryResource : IGalleryResource
{
    private readonly HttpClient _httpClient;

    public GalleryResource(HttpClient httpClient) =>
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

    public async Task<IList<string>> GetGallerySubmissionLinks(string cookie, CancellationToken ct = default)
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, "/browse");
        requestMessage.AddCookie(cookie);

        using var responseMessage = await _httpClient.SendAsync(requestMessage, ct);

        var response = await responseMessage.Content.ReadAsStringAsync(ct);

        var parser = new GallerySubmissionsParser();

        var result = parser.GetGallerySubmissionLinks(response);

        return result;
    }
}