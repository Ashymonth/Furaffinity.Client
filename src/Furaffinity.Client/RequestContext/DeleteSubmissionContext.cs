using Furaffinity.Client.Models;

namespace Furaffinity.Client.RequestContext;

internal class DeleteSubmissionContext
{
    public SubmissionId SubmissionId { get; set; } = null!;
    
    public string FormKey { get; set; } = null!;

    public string AccountPassword { get; set; } = null!;

    public bool IsSuccessfully { get; set; }
}