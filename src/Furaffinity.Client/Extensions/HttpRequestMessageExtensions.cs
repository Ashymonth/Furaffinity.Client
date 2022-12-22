namespace Furaffinity.Client.Extensions;

internal static class HttpRequestMessageExtensions
{
    public static void AddCookie(this HttpRequestMessage message, string cookie)
    {
        message.Options.Set(new HttpRequestOptionsKey<string>("Cookie"), cookie);
    }
}