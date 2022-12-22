using System.Net.Http.Headers;
using Furaffinity.Client.Extensions;
using Furaffinity.Client.Helpers;
using Furaffinity.Client.Models;

namespace Furaffinity.Client.Requests;

internal class UploadSubmissionsRequest : IRequest
{
    public UploadSubmissionsRequest(string cookie, string formKey, UploadFile uploadFile)
    {
        var content = new MultipartFormDataContent();
        content.AddFormDataContent("MAX_FILE_SIZE", "10485760");
        content.AddFormDataContent("key", formKey);
        content.AddFormDataContent("submission_type", uploadFile.SubmissionTypeId);
        
        AddFile(content, uploadFile);

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/submit/upload/");
        requestMessage.Content = content;

        requestMessage.AddCookie(cookie);

        RequestMessage = requestMessage;
    }
    
    public HttpRequestMessage RequestMessage { get; }

    public void Dispose()
    {
        RequestMessage.Dispose();
    }
    
    private static void AddFile(MultipartContent dataContent, UploadFile file)
    {
        //TODO thumbnail
        var stream = new MemoryStream(file.Data);
        var content = new StreamContent(stream);
        content.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse("form-data");
        content.Headers.ContentDisposition.Name = file.SubmissionTypeId;
        content.Headers.ContentDisposition.FileName = file.FileName;
        content.Headers.ContentType =
            MediaTypeHeaderValue.Parse(ContentTypeMap.GetContentTypeByFileExtension(file.Extension));
        dataContent.Add(content);
    }
}