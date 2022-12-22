using Furaffinity.Client.Resources;

namespace Furaffinity.Client;

/// <summary>
/// Provide access to api.
/// </summary>
public interface IFuraffinityClient
{
    /// <summary>
    /// Manage account actions.
    /// </summary>
    IAccountResource Account { get; }

    /// <summary>
    /// Manage gallery actions.
    /// </summary>
    IGalleryResource Gallery { get; }

    /// <summary>
    /// Manage submission actions
    /// </summary>
    ISubmissionResource Submission { get; }
}

/// <summary>
/// <inheritdoc cref="IFuraffinityClient"/>
/// </summary>
internal class FuraffinityClient : IFuraffinityClient
{
    public FuraffinityClient(IGalleryResource gallery,
        ISubmissionResource submissionResource,
        IAccountResource accountResource)
    {
        Gallery = gallery ?? throw new ArgumentNullException(nameof(gallery));
        Submission = submissionResource ?? throw new ArgumentNullException(nameof(submissionResource));
        Account = accountResource ?? throw new ArgumentNullException(nameof(accountResource));
    }

    public IAccountResource Account { get; }
    
    public IGalleryResource Gallery { get; }

    public ISubmissionResource Submission { get; }
}