using Furaffinity.Client.Extensions;
using Furaffinity.Client.Parsers;
using Furaffinity.Client.RequestContext;
using Furaffinity.Client.Requests;

namespace Furaffinity.Client.SubmissionActions.SubmissionDeleteActions;

internal class ConfirmDeleteSubmissionAction : ISubmissionDeleteAction
{
    private readonly HttpClient _httpClient;

    public ConfirmDeleteSubmissionAction(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public int Order => 2;

    public async Task ExecuteAsync(string cookie, DeleteSubmissionContext context, CancellationToken ct = default)
    {
        using var requestMessage =
            new ConfirmDeleteSubmissionRequest(cookie, context.FormKey, context.AccountPassword, context.SubmissionId);
        
        using var responseMessage = await _httpClient.SendAsync(requestMessage.RequestMessage, ct);

        var response = await responseMessage.Content.ReadAsStringAsync(ct);

        var parser = new AccountManageSubmissionParser();
 
        // confirm, that we redirected and submission actually deleted;
        context.IsSuccessfully = parser.HasAnySubmissions(response); 
    }
}