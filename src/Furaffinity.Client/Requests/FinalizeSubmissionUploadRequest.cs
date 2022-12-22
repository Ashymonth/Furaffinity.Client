using Furaffinity.Client.Contracts;
using Furaffinity.Client.Extensions;

namespace Furaffinity.Client.Requests;

internal class FinalizeSubmissionUploadRequest: IRequest
{
    public FinalizeSubmissionUploadRequest(string cookie, string formDataKey, Submission submission)
    {
        var content = new MultipartFormDataContent();
      
        content.AddFormDataContent("key", formDataKey);
        content.AddFormDataContent("cat", submission.Category.CategoryId);
        content.AddFormDataContent("atype", submission.Theme.ThemeId);
        content.AddFormDataContent("species", submission.Species.SpeciesId);
        content.AddFormDataContent("gender", submission.Gender.GenderId);
        content.AddFormDataContent("rating", submission.Rating.RatingId);
        content.AddFormDataContent("title", submission.Title.Value);
        content.AddFormDataContent("message", submission.Description ?? string.Empty);
        content.AddFormDataContent("keywords", submission.Keywords ?? string.Empty);
        content.AddFormDataContent("lock_comments", submission.Comments.Id);
        content.AddFormDataContent("scrap", submission.PutInScrap.Id);
        content.AddFormDataContent("create_folder_name", submission.NewFolderName ?? string.Empty);
        //folder_ids[] - если указывать, что работу нужно положить в папку

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/submit/finalize/");
        requestMessage.Content = content;

        requestMessage.AddCookie(cookie);

        RequestMessage = requestMessage;
    }
    
    public HttpRequestMessage RequestMessage { get; }

    public void Dispose()
    {
        RequestMessage.Dispose();
    }
}