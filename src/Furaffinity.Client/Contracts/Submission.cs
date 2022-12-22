using Furaffinity.Client.Models;

namespace Furaffinity.Client.Contracts;

/// <summary>
/// Submission (artwork, story, etc)
/// </summary>
public class Submission
{
    private Submission()
    {
    }

    /// <summary>
    /// Submission file to upload.
    /// </summary>
    public UploadFile File { get; internal set; } = null!;

    // public SubmissionFile? Thumbnail { get; set; }

    /// <summary>
    /// Submission rating.
    /// </summary>
    public Rating Rating { get; internal set; } = null!;

    /// <summary>
    /// Submission title.
    /// </summary>
    public Title Title { get; internal set; } = null!;

    /// <summary>
    /// Submission description.
    /// </summary>
    public string? Description { get; internal set; }

    /// <summary>
    /// Submission keywords.
    /// </summary>
    public string? Keywords { get; internal set; }

    /// <summary>
    /// Submission category.
    /// </summary>
    public Category Category { get; internal set; } = null!;

    /// <summary>
    /// Submission theme.
    /// </summary>
    public Theme Theme { get; internal set; } = null!;

    /// <summary>
    /// Submission species.
    /// </summary>
    public Species Species { get; internal set; } = null!;

    /// <summary>
    /// Submission gender.
    /// </summary>
    public Gender Gender { get; internal set; } = null!;

    /// <summary>
    /// Folder name to create and put submission in this folder.
    /// </summary>
    public string? NewFolderName { get; internal set; }

    //public string[] PutInFolders { get; set; }

    /// <summary>
    /// Enable or disable comments.
    /// </summary>
    public Comments Comments { get; internal set; } = Comments.Enabled();

    /// <summary>
    /// Enable or disable put in scraps.
    /// </summary>
    public PutInScrap PutInScrap { get; internal set; } = PutInScrap.Disabled();

    /// <summary>
    /// Return builder for creating submission model.
    /// </summary>
    /// <returns></returns>
    public static SubmissionBuilder CreateBuilder() => new(new Submission());
}