using System.Runtime.CompilerServices;

namespace Furaffinity.Client.Helpers;

internal static class ContentTypeMap
{
    private static readonly Dictionary<string, string> FileExtensionToContentType = new()
    {
        // Artwork
        {".png", "image/png"},
        {".jpg", "image/jpeg"},
        {".jpeg", "image/jpeg"},
        {".gif", "image/gif"},

        // Story and Poetry
        {".txt", "text/plain"},
        {".doc", "application/octet-stream"},
        {".docx", "application/octet-stream"},
        {".odt", "application/octet-stream"},
        {".rtf", "application/octet-stream"},
        {".pdf", "application/pdf"},

        // Music
        {".mp3", "audio/mpeg"},
        {".wav", "audio/wav"},
        {".mid", "audio/mid"},
    };

    public static string GetContentTypeByFileExtension(string fileExtension,
        [CallerArgumentExpression("fileExtension")] string message = null!)
    {
        if (FileExtensionToContentType.TryGetValue(fileExtension.ToLower(), out var value))
        {
            return value;
        }

        throw new ArgumentException($"File extension: {fileExtension} is not supported", nameof(fileExtension));
    }
}