using Furaffinity.Client.Constants;

namespace Furaffinity.Client.Models;

/// <summary>
/// Submission file.
/// </summary>
public class UploadFile : IFile, IEqualityComparer<UploadFile>
{
    private const long MaxFileSizeInBytes = 10485760;

    private static readonly string[] ImageExtensions = {".jpg", ".jpeg", ".png", ".gif"};
    private static readonly string[] DocumentExtensions = {".txt", ".doc", ".odt", ".rtf", ".pdf"};
    private static readonly string[] MusicExtensions = {".mp3", ".wav", ".mid"};

    private static readonly Dictionary<string, string[]> SubmissionTypeToAllowExtensionMap = new()
    {
        {SubmissionTypeName.Artwork.ToLower(), ImageExtensions},
        {SubmissionTypeName.Story.ToLower(), DocumentExtensions},
        {SubmissionTypeName.Poetry.ToLower(), DocumentExtensions},
        {SubmissionTypeName.Music.ToLower(), MusicExtensions},
    };

    private static readonly Dictionary<string, string> SubmissionNameToIdMap = new()
    {
        {SubmissionTypeName.Artwork.ToLower(), "submission"},
        {SubmissionTypeName.Story.ToLower(), "story"},
        {SubmissionTypeName.Poetry.ToLower(), "poetry"},
        {SubmissionTypeName.Music.ToLower(), "music"},
    };

    internal UploadFile(string submissionType, string fileName, byte[] data)
    {
        if (string.IsNullOrWhiteSpace(submissionType))
        {
            throw new ArgumentNullException(nameof(submissionType));
        }

        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentNullException(nameof(fileName));
        }

        if (data is null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        if (data.Length == 0)
        {
            throw new ArgumentException("Upload file data can't be empty", nameof(data));
        }

        if (data.Length > MaxFileSizeInBytes)
        {
            throw new InvalidOperationException($"File: {fileName} is more that 10 mb");
        }

        var fileExtension = Path.GetExtension(fileName);
        if (string.IsNullOrWhiteSpace(fileExtension))
        {
            throw new InvalidOperationException($"Unable to get file extension from file name: {fileName}");
        }

        if (!SubmissionNameToIdMap.TryGetValue(submissionType.ToLower(), out var submissionId))
        {
            throw new InvalidOperationException($"Submission type: {submissionType} invalid");
        }

        ValidateFileExtension(submissionType, fileExtension);

        SubmissionTypeId = submissionId;
        FileName = fileName;
        Data = data;
        Extension = fileExtension;
    }

    /// <summary>
    /// Submission type
    /// <remarks><see cref="SubmissionTypeName"/></remarks>
    /// </summary>
    public string SubmissionTypeId { get; }

    /// <summary>
    /// Submission file name.
    /// </summary>
    public string FileName { get; }

    /// <summary>
    /// Submission file data
    /// </summary>
    public byte[] Data { get; }

    /// <summary>
    /// File extension.
    /// </summary>
    public string Extension { get; }

    private static void ValidateFileExtension(string submissionType, string fileExtension)
    {
        if (!SubmissionTypeToAllowExtensionMap.TryGetValue(submissionType.ToLower(), out var allowedFileExtensions))
        {
            throw new InvalidOperationException($"Submission type: {submissionType} is not supported");
        }

        Validate(submissionType, fileExtension, allowedFileExtensions);
    }

    private static void Validate(string submissionType, string fileExtension, string[] allowedExtensions)
    {
        if (!allowedExtensions.Contains(fileExtension))
        {
            throw new InvalidOperationException(
                $"For {submissionType} file type must be: {string.Join(",", allowedExtensions)}");
        }
    }

    /// <inheritdoc />
    public bool Equals(UploadFile? x, UploadFile? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.SubmissionTypeId == y.SubmissionTypeId && x.FileName == y.FileName && x.Data.Equals(y.Data) &&
               x.Extension == y.Extension;
    }

    /// <inheritdoc />
    public int GetHashCode(UploadFile obj)
    {
        return HashCode.Combine(obj.SubmissionTypeId, obj.FileName, obj.Data, obj.Extension);
    }
}