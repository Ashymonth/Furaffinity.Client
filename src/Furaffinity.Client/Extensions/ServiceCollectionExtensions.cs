using Furaffinity.Client.Handlers;
using Furaffinity.Client.Parsers;
using Furaffinity.Client.Resources;
using Furaffinity.Client.SubmissionActions.SubmissionDeleteActions;
using Furaffinity.Client.SubmissionActions.SubmissionDetailsActions;
using Furaffinity.Client.SubmissionActions.SubmissionFavActions;
using Furaffinity.Client.SubmissionActions.SubmissionUploadActions;
using Microsoft.Extensions.DependencyInjection;

namespace Furaffinity.Client.Extensions;

/// <summary>
/// Extensions for client registration.
/// </summary>
public static class ServiceCollectionExtensions
{
    private const string BaseAddress = "https://www.furaffinity.net/";
    private const string DefaultClientName = "DefaultClient";
    private const string CookieClientName = "CookieClient";
    private const string DownloadClientName = "DownloadClient";

    /// <summary>
    /// Add furaffinity client. <see cref="IFuraffinityClient"/>.
    /// </summary>
    /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add services to.</param>
    /// <returns></returns>
    public static IServiceCollection AddFuraffinityClient(this IServiceCollection services)
    {
        services.AddHttpClient(DownloadClientName);
        services.AddHttpClient(DefaultClientName, client => client.BaseAddress = new Uri(BaseAddress));
        services.AddHttpClient(CookieClientName, client => client.BaseAddress = new Uri(BaseAddress))
            .ConfigurePrimaryHttpMessageHandler(_ => new CookieHandler());

        services.AddSingleton<ErrorParser>();

        AddSubmissionResource(services);

        AddGalleryResource(services);

        AddAccountResource(services);

        services.AddScoped<IFuraffinityClient, FuraffinityClient>();
        services.AddScoped<IDownloadClient, DownloadClient>(provider =>
        {
            var factory = provider.GetRequiredService<IHttpClientFactory>();
            return new DownloadClient(factory.CreateClient(DownloadClientName));
        });

        return services;
    }

    private static void AddGalleryResource(IServiceCollection services)
    {
        services.AddScoped<IGalleryResource, GalleryResource>(provider =>
        {
            var client = provider.GetRequiredService<IHttpClientFactory>().CreateClient(CookieClientName);
            var galleryResource = new GalleryResource(client);

            return galleryResource;
        });
    }

    private static void AddAccountResource(IServiceCollection services)
    {
        AddSubmissionUploadActions(services);

        AddSubmissionDeleteActions(services);
        
        services.AddScoped<IAccountResource, AccountResource>(provider =>
        {
            var defaultClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient(CookieClientName);
            var downloadClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient(DownloadClientName);

            var uploadActions = provider.GetServices<ISubmissionUploadAction>()
                .OrderBy(action => action.Order)
                .ToArray();

            var deleteActions = provider.GetServices<ISubmissionDeleteAction>()
                .OrderBy(action => action.Order)
                .ToArray();

            return new AccountResource(defaultClient, downloadClient, uploadActions, deleteActions);
        });
    }

    private static IServiceCollection AddSubmissionResource(IServiceCollection services)
    {
        AddSubmissionsDetailsActions(services);

        AddFavActions(services);

        services.AddScoped<ISubmissionResource, SubmissionResource>(provider =>
        {
            var detailsActions = provider.GetServices<ISubmissionDetailsAction>()
                .OrderBy(action => action.Order)
                .ToArray();

            var favActions = provider.GetServices<IFavAction>()
                .OrderBy(action => action.Order)
                .ToArray();

            var defaultClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient(DefaultClientName);

            var resource = new SubmissionResource(detailsActions, favActions, new DownloadClient(defaultClient), defaultClient);

            return resource;
        });

        return services;
    }

    private static void AddFavActions(IServiceCollection services)
    {
        services.AddScoped<IFavAction, GetFavLink>(provider => new GetFavLink(GetCookieClient(provider)));
        services.AddScoped<IFavAction, FavSubmissionAction>(provider =>
            new FavSubmissionAction(GetCookieClient(provider)));
    }

    private static void AddSubmissionsDetailsActions(IServiceCollection services)
    {
        services.AddScoped<ISubmissionDetailsAction, GetAccountLinkId>(provider =>
            new GetAccountLinkId(GetCookieClient(provider)));

        services.AddScoped<ISubmissionDetailsAction, GetAccountFileId>(provider =>
            new GetAccountFileId(GetCookieClient(provider)));

        services.AddScoped<ISubmissionDetailsAction, GetSubmissionDetails>(provider =>
            new GetSubmissionDetails(GetCookieClient(provider)));
    }

    private static void AddSubmissionUploadActions(IServiceCollection services)
    {
        services.AddScoped<ISubmissionUploadAction, GetInitialFormKeyAction>(provider =>
            new GetInitialFormKeyAction(GetCookieClient(provider)));

        services.AddScoped<ISubmissionUploadAction, UploadSubmissionAction>(provider =>
            new UploadSubmissionAction(GetCookieClient(provider)));

        services.AddScoped<ISubmissionUploadAction, FinalizeSubmissionsAction>(provider =>
            new FinalizeSubmissionsAction(GetCookieClient(provider)));
    }

    private static void AddSubmissionDeleteActions(IServiceCollection services)
    {
        services.AddScoped<ISubmissionDeleteAction, DeleteSubmissionAction>(provider =>
            new DeleteSubmissionAction(GetCookieClient(provider)));

        services.AddScoped<ISubmissionDeleteAction, ConfirmDeleteSubmissionAction>(provider =>
            new ConfirmDeleteSubmissionAction(GetCookieClient(provider)));
    }

    private static HttpClient GetCookieClient(IServiceProvider provider) =>
        provider.GetRequiredService<IHttpClientFactory>().CreateClient(CookieClientName);
}