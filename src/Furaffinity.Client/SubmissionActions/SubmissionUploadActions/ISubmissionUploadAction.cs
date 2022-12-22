using Furaffinity.Client.RequestContext;

namespace Furaffinity.Client.SubmissionActions.SubmissionUploadActions;

internal interface ISubmissionUploadAction : IPageAction
{
    Task ExecuteAsync(string cookie, SubmissionUploadContext context, CancellationToken ct);
} 