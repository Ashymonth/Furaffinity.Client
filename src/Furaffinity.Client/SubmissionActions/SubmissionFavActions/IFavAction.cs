using Furaffinity.Client.RequestContext;

namespace Furaffinity.Client.SubmissionActions.SubmissionFavActions;

internal interface IFavAction : IPageAction
{
    Task ExecuteAsync(string cookie, FavContext context, CancellationToken ct = default);
}