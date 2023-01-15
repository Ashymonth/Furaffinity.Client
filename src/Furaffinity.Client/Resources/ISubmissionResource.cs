using Furaffinity.Client.Contracts;
using Furaffinity.Client.Models;
using Furaffinity.Client.Parsers;
using Furaffinity.Client.RequestContext;
using Furaffinity.Client.SubmissionActions.SubmissionDetailsActions;
using Furaffinity.Client.SubmissionActions.SubmissionFavActions;

namespace Furaffinity.Client.Resources;

/// <summary>
/// Access to action with submissions.
/// </summary>
public interface ISubmissionResource
{
    /// <summary>
    /// Fav selected submission by id.
    /// </summary>
    /// <param name="cookie">Account authentication cookie.</param>
    /// <param name="submissionId">Submission identifier.</param>
    /// <param name="ct"> A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Return true if fav succeed</returns>
    Task<bool> FavSubmissionAsync(string cookie, SubmissionId? submissionId, CancellationToken ct = default);

    /// <summary>
    /// Get submission statistic.
    /// </summary>
    /// <param name="submissionId">Submission identifier.</param>
    /// <param name="ct"> A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns><see cref="SubmissionStatistic"/></returns>
    Task<SubmissionStatistic> GetSubmissionsStatisticAsync(SubmissionId submissionId,
        CancellationToken ct = default);

    /// <summary>
    /// Get submission categories (theme, gender, etc).
    /// </summary>
    /// <param name="cookie">Account authentication cookie.</param>
    /// <param name="ct"> A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns></returns>
    Task<Dictionary<string, List<CategoryResponse>>> GetCategoriesAsync(string cookie,
        CancellationToken ct = default);

    /// <summary>
    /// Get submission info from site by submission id
    /// </summary>
    /// <param name="submissionId">Submission identifier</param>
    /// <param name="ct">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns></returns>
    Task<Submission> GetSubmissionById(SubmissionId submissionId, CancellationToken ct = default);
}

/// <summary>
/// <inheritdoc cref="ISubmissionResource"/>
/// </summary>
internal class SubmissionResource : ISubmissionResource
{
    private readonly ISubmissionDetailsAction[] _detailsActions;
    private readonly IFavAction[] _favActions;
    private readonly IDownloadClient _downloadClient;
    private readonly HttpClient _httpClient;

    public SubmissionResource(ISubmissionDetailsAction[] detailsActions,
        IFavAction[] favActions,
        IDownloadClient downloadClient,
        HttpClient httpClient)
    {
        _detailsActions = detailsActions ?? throw new ArgumentNullException(nameof(detailsActions));
        _favActions = favActions ?? throw new ArgumentNullException(nameof(favActions));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _downloadClient = downloadClient ?? throw new ArgumentNullException(nameof(downloadClient));
    }

    public async Task<bool> FavSubmissionAsync(string cookie,
        SubmissionId? submissionId,
        CancellationToken ct = default)
    {
        if (submissionId is null)
        {
            throw new ArgumentNullException(nameof(submissionId));
        }

        var context = new FavContext {SubmissionUrl = $"/view/{submissionId.Value}/"};
        foreach (var favAction in _favActions.OrderBy(action => action.Order))
        {
            await favAction.ExecuteAsync(cookie, context, ct);
        }

        return context.IsSucceed;
    }

    public async Task<SubmissionStatistic> GetSubmissionsStatisticAsync(SubmissionId submissionId,
        CancellationToken ct = default)
    {
        using var responseMessage = await _httpClient.GetAsync($"/view/{submissionId.Value}/", ct);

        var response = await responseMessage.Content.ReadAsStringAsync(ct);

        var statisticParser = new SubmissionStatisticParser();

        var result = statisticParser.GetSubmissionStatistic(response);

        return result;
    }

    public async Task<Dictionary<string, List<CategoryResponse>>> GetCategoriesAsync(string cookie,
        CancellationToken ct = default)
    {
        var context = new SubmissionDetailsContext();
        foreach (var detailsAction in _detailsActions.OrderBy(action => action.Order))
        {
            await detailsAction.ExecuteAsync(cookie, context, ct);
        }

        return context.SubmissionsDetails;
    }

    public async Task<Submission> GetSubmissionById(SubmissionId submissionId, CancellationToken ct = default)
    {
        using var responseMessage = await _httpClient.GetAsync($"/view/{submissionId.Value}/", ct);

        var page = await responseMessage.Content.ReadAsStringAsync(ct);

        ErrorParser.ValidateSubmissionPage(page);
        
        var parser = new SubmissionInfoParser(_downloadClient);

        var result = await parser.GetSubmissionAsync(page);

        return result;
    }
}