using System.Net.Http.Headers;

namespace Furaffinity.Client.Extensions;

internal static class MultipartFormDataContentExtensions
{
    private const string FormData = "form-data";

    // we can't pass value by other way. site don't accept it.
    public static void AddFormDataContent(this MultipartContent dataContent, string value, string name)
    {
        var content = new StringContent(name);
        content.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse(FormData);
        content.Headers.ContentDisposition.Name = value;
        content.Headers.ContentType = null;
        dataContent.Add(content);
    }
}