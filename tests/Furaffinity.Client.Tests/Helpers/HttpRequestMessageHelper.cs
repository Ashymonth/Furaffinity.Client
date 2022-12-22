using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Furaffinity.Client.Tests.Helpers;

internal static class HttpRequestMessageHelper
{
    public static async Task<HttpRequestMessage> CloneAsync(this HttpRequestMessage message)
    {
        var messageClone = new HttpRequestMessage(HttpMethod.Post, message.RequestUri);

        var mss = new MemoryStream();
        await message.Content!.CopyToAsync(mss);

        mss.Seek(0, SeekOrigin.Begin);
        messageClone.Content = new StreamContent(mss);

        return messageClone;
    }
}