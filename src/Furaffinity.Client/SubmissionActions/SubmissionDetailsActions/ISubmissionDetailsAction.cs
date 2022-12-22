using Furaffinity.Client.RequestContext;

namespace Furaffinity.Client.SubmissionActions.SubmissionDetailsActions;

internal interface ISubmissionDetailsAction : IPageAction
{
    Task ExecuteAsync(string cookie, SubmissionDetailsContext context, CancellationToken ct = default);
}