using Furaffinity.Client.Extensions;
using Furaffinity.Client.Models;

namespace Furaffinity.Client.Requests;

internal class DeleteSubmissionRequest : IDisposable
{
    public DeleteSubmissionRequest(SubmissionId submissionId, string cookie)
    {
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("submission_ids[]", submissionId.Value),
            new KeyValuePair<string, string>("delete_submissions_submit", "1")
        });

        RequestMessage = new HttpRequestMessage(HttpMethod.Post, "/controls/submissions/");
        RequestMessage.Content = content;
        
        RequestMessage.AddCookie(cookie);
    }

    public HttpRequestMessage RequestMessage { get; }

    public void Dispose()
    {
        RequestMessage.Dispose();
    }
}