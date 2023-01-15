using Furaffinity.Client.Constants;
using Furaffinity.Client.Models;

namespace Furaffinity.Client;

/// <summary>
/// Client for download
/// </summary>
internal interface IDownloadClient
{
    /// <summary>
    /// Get profile avatar data.
    /// </summary>
    /// <param name="avatarUrl">Link to avatar image.</param>
    /// <param name="ct"><see cref="CancellationToken"/></param>
    /// <returns></returns>
    Task<byte[]> DownloadAvatarUrl(string avatarUrl, CancellationToken ct = default);
    
    /// <summary>
    /// Get submission file.
    /// </summary>
    /// <param name="submissionUrl">Link to submission</param>
    /// <param name="ct"><see cref="CancellationToken"/></param>
    /// <returns></returns>
    Task<UploadFile> DownloadSubmissionFile(string submissionUrl, CancellationToken ct = default);
}

/// <summary>
/// <see cref="IDownloadClient"/>
/// </summary>
internal class DownloadClient : IDownloadClient
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Create a new instance of <see cref="DownloadClient"/>
    /// </summary>
    /// <param name="httpClient"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public DownloadClient(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<byte[]> DownloadAvatarUrl(string avatarUrl, CancellationToken ct = default)
    {
        //unable to use GetByArrayAsync, because sometimes server return image with 404 status.
        var result = await _httpClient.GetAsync($"https:{avatarUrl}", ct);

        return await result.Content.ReadAsByteArrayAsync(ct);
    }

    public async Task<UploadFile> DownloadSubmissionFile(string submissionUrl, CancellationToken ct = default)
    {
        var result = await _httpClient.GetAsync($"https:{submissionUrl}", ct);

        var data = await result.Content.ReadAsByteArrayAsync(ct);
        
        var submissionType = ExtractSubmissionType(submissionUrl);

        var lastSlashIndex = submissionUrl.LastIndexOf("/", StringComparison.Ordinal);

        var submissionName = submissionUrl[(lastSlashIndex + 1)..];

        return new UploadFile(submissionType, submissionName, data);
    }

    private static string ExtractSubmissionType(string submissionUrl)
    {
        string submissionType;
        if (submissionUrl.Contains(SubmissionTypeName.Music, StringComparison.OrdinalIgnoreCase))
        {
            submissionType = SubmissionTypeName.Music;
        }
        else if (submissionUrl.Contains(SubmissionTypeName.Story, StringComparison.OrdinalIgnoreCase))
        {
            submissionType = SubmissionTypeName.Story;
        }
        else if (submissionUrl.Contains(SubmissionTypeName.Poetry, StringComparison.OrdinalIgnoreCase))
        {
            submissionType = SubmissionTypeName.Poetry;
        }
        else
        {
            submissionType = SubmissionTypeName.Artwork;
        }

        return submissionType;
    }
}