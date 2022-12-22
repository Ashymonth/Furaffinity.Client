using Furaffinity.Client.RequestContext;

namespace Furaffinity.Client.SubmissionActions.SubmissionDeleteActions;

internal interface ISubmissionDeleteAction : IPageAction
{
    Task ExecuteAsync(string cookie, DeleteSubmissionContext context, CancellationToken ct = default);
}