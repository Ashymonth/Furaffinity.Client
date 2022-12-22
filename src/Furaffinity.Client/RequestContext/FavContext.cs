namespace Furaffinity.Client.RequestContext;

internal class FavContext
{
    public string SubmissionUrl { get; set; } = null!;
    
    public string FavUrl { get; set; } = null!;

    public bool IsSucceed { get; set; }
}