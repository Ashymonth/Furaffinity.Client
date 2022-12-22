using Furaffinity.Client.Parsers;
using Furaffinity.Client.RequestContext;
using Furaffinity.Client.Requests;

namespace Furaffinity.Client.SubmissionActions.SubmissionDeleteActions;

internal class DeleteSubmissionAction : ISubmissionDeleteAction
{
    private readonly HttpClient _httpClient;

    public DeleteSubmissionAction(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public int Order => 1;

    public async Task ExecuteAsync(string cookie, DeleteSubmissionContext context, CancellationToken ct = default)
    {
        using var deleteSubmissionRequest = new DeleteSubmissionRequest(context.SubmissionId, cookie);
        
        using var responseMessage = await _httpClient.SendAsync(deleteSubmissionRequest.RequestMessage, ct);

        var response = await responseMessage.Content.ReadAsStringAsync(ct);

        var formKeyParser = new ConfirmDeleteSubmissionFormKeyParser();

        var formKey = formKeyParser.GetFormKey(response);

        context.FormKey = formKey;
    }
}