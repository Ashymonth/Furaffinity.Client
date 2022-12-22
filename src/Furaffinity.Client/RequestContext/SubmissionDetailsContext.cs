using Furaffinity.Client.Contracts;

namespace Furaffinity.Client.RequestContext;

internal class SubmissionDetailsContext
{
    public string AccountLink { get; set; } = null!;

    public string SubmissionId { get; set; } = null!;

    public Dictionary<string, List<CategoryResponse>> SubmissionsDetails { get; internal set; } = new();
}