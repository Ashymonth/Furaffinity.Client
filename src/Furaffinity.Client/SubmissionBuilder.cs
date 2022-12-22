using Furaffinity.Client.Constants;
using Furaffinity.Client.Contracts;
using Furaffinity.Client.Models;

namespace Furaffinity.Client;

/// <summary>
/// Accesses to create submission.
/// </summary>
public class SubmissionBuilder
{
    private const int KeywordsMaxLength = 250;

    private readonly Submission _submission;

    /// <summary>
    /// Create a new instance of <see cref="Submission"/>
    /// </summary>
    /// <param name="submission">Submission to create</param>
    /// <exception cref="ArgumentNullException">if submission is null.</exception>
    internal SubmissionBuilder(Submission submission)
    {
        _submission = submission ?? throw new ArgumentNullException(nameof(submission));
    }

    /// <summary>
    /// Set submission title.
    /// </summary>
    /// <param name="title">Submission title.</param>
    /// <returns></returns>
    public SubmissionBuilder SetTitle(string? title)
    {
        _submission.Title = new Title(title);
        return this;
    }

    /// <summary>
    /// Set submission description.
    /// </summary>
    /// <param name="description">Submission description.</param>
    /// <returns></returns>
    public SubmissionBuilder SetDescription(string? description)
    {
        _submission.Description = description;
        return this;
    }

    /// <summary>
    /// Set submission keywords.
    /// </summary>
    /// <param name="keywords">Submission keywords.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public SubmissionBuilder SetKeywords(string? keywords)
    {
        if (keywords?.Length > KeywordsMaxLength)
        {
            throw new ArgumentException($"Title can't be more that {KeywordsMaxLength} characters", nameof(keywords));
        }

        _submission.Keywords = keywords;
        return this;
    }

    /// <summary>
    /// Set submission file to upload.
    /// </summary>
    /// <param name="submissionType">Submission type.</param>
    /// <param name="fileName">File name.</param>
    /// <param name="data">File data.</param>
    /// <remarks><see cref="SubmissionTypeName"/></remarks>
    /// <returns></returns>
    public SubmissionBuilder SetFile(string? submissionType, string? fileName, byte[] data)
    {
        _submission.File = new UploadFile(submissionType, fileName, data);
        return this;
    }

    /// <summary>
    /// Set submission rating. 
    /// </summary>
    /// <param name="ratingName">Submission rating.</param>
    /// <remarks><see cref="SubmissionRatingName"/></remarks>
    /// <returns></returns>
    public SubmissionBuilder SetRating(string? ratingName)
    {
        _submission.Rating = new Rating(ratingName);
        return this;
    }

    /// <summary>
    /// Set submission theme.
    /// </summary>
    /// <param name="themeName">Submission theme.</param>
    /// <returns></returns>
    public SubmissionBuilder SetTheme(string? themeName)
    {
        _submission.Theme = new Theme(themeName);
        return this;
    }

    /// <summary>
    /// Set submission category.
    /// </summary>
    /// <param name="categoryName">Submission category./</param>
    /// <returns></returns>
    public SubmissionBuilder SetCategory(string? categoryName)
    {
        _submission.Category = new Category(categoryName);
        return this;
    }

    /// <summary>
    /// Set submission gender.
    /// </summary>
    /// <param name="genderName">Submission gender.</param>
    /// <returns></returns>
    public SubmissionBuilder SetGender(string? genderName)
    {
        _submission.Gender = new Gender(genderName);
        return this;
    }

    /// <summary>
    /// Set submission species.
    /// </summary>
    /// <param name="speciesName">Submission species.</param>
    /// <returns></returns>
    public SubmissionBuilder SetSpecies(string? speciesName)
    {
        _submission.Species = new Species(speciesName);
        return this;
    }

    /// <summary>
    /// Enable or disable comments.
    /// </summary>
    /// <param name="comments">Submission comment value.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">if comments are null.</exception>
    public SubmissionBuilder SetComments(Comments? comments)
    {
        if (comments is null)
        {
            throw new ArgumentNullException(nameof(comments));
        }

        _submission.Comments = comments;
        return this;
    }

    /// <summary>
    /// Enable or disable put in scrap.
    /// </summary>
    /// <param name="putInScrap">Submission put in scrap value.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">If putInScrap is null.</exception>
    public SubmissionBuilder PutInScraps(PutInScrap? putInScrap)
    {
        if (putInScrap is null)
        {
            throw new ArgumentNullException(nameof(putInScrap));
        }

        _submission.PutInScrap = putInScrap;
        return this;
    }

    /// <summary>
    /// Set folder name.
    /// </summary>
    /// <param name="folderName">Folder name.</param>
    /// <remarks>If folder not exist, then folder will be created and submission will be upload to this folder.</remarks>
    /// <returns></returns>
    public SubmissionBuilder SetFolderName(string? folderName)
    {
        _submission.NewFolderName = folderName;
        return this;
    }

    /// <summary>
    /// Create submission.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">If <see cref="Title"/>
    /// or <see cref="UploadFile"/>
    /// or <see cref="Rating"/>
    /// or <see cref="Gender"/>
    /// or <see cref="Species"/>
    /// or <see cref="Theme"/>
    /// or <see cref="Category"/> is null
    /// </exception>
    public Submission Build()
    {
        if (_submission.Title is null)
        {
            throw new InvalidOperationException("Title can't be empty");
        }

        if (_submission.File is null)
        {
            throw new InvalidOperationException("Upload file required");
        }

        if (_submission.Rating is null)
        {
            throw new InvalidOperationException("Rating for submission is required");
        }

        if (_submission.Gender is null)
        {
            throw new InvalidOperationException("Gender for submission is required");
        }

        if (_submission.Species is null)
        {
            throw new InvalidOperationException("Species for submission is required");
        }

        if (_submission.Theme is null)
        {
            throw new InvalidOperationException("Theme for submission is required");
        }

        if (_submission.Category is null)
        {
            throw new InvalidOperationException("Category for submission is required");
        }

        return _submission;
    }
}