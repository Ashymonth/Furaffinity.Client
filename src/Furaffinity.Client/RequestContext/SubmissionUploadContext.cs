using Furaffinity.Client.Contracts;

namespace Furaffinity.Client.RequestContext;

internal class SubmissionUploadContext
{
    public Submission Submission { get; set; } = null!;
    
    public string FormKey { get; set; } = null!;

    public string SubmissionId { get; set; } = null!;
}