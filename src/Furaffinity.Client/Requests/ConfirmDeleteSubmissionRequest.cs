using Furaffinity.Client.Extensions;
using Furaffinity.Client.Models;

namespace Furaffinity.Client.Requests;

internal class ConfirmDeleteSubmissionRequest : IRequest
{
    public ConfirmDeleteSubmissionRequest(string cookie, string formKey, string password, SubmissionId submissionId)
    {
        var requestContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("delete_submissions_submit", "1"),
            new KeyValuePair<string, string>("submission_ids[]", submissionId.Value),
            new KeyValuePair<string, string>("password", password),
            new KeyValuePair<string, string>("confirm", formKey)
        });
        
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/controls/submissions/");
        requestMessage.Content = requestContent;
        
        requestMessage.AddCookie(cookie);

        RequestMessage = requestMessage;
    }
    
    public HttpRequestMessage RequestMessage { get; }
    
    public void Dispose()
    {
        RequestMessage.Dispose();
    }
}