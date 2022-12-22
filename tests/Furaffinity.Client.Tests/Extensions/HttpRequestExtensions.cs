using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Furaffinity.Client.Tests.Helpers;

namespace Furaffinity.Client.Tests.Extensions;

internal static class HttpRequestExtensions
{
    public static async Task<bool> CompareRequestsAsync(this HttpRequestMessage originalRequest,
        HttpRequestMessage requestToCompare)
    {
        var isRequestTheSame =
            originalRequest.RequestUri!.AbsolutePath == requestToCompare.RequestUri!.OriginalString &&
            originalRequest.Method == requestToCompare.Method;

        switch (requestToCompare.Content)
        {
            case FormUrlEncodedContent:
            {
                using var requestClone = await originalRequest.CloneAsync();

                var originalContent = await requestClone.Content!.ReadAsStringAsync();

                var contentToCompare = await requestToCompare.Content!.ReadAsStringAsync();

                return isRequestTheSame && originalContent == contentToCompare;
            }
            case MultipartFormDataContent formDataContent:
            {
                var toCompareHeader = formDataContent
                    .Select(toCompareContent => toCompareContent.Headers.ContentDisposition).ToHashSet();

                var originalHeaders = ((MultipartFormDataContent) originalRequest.Content!)
                    .Select(toCompareContent => toCompareContent.Headers.ContentDisposition).ToHashSet();

                return isRequestTheSame && toCompareHeader.SequenceEqual(originalHeaders);
            }
            default:
                throw new ArgumentException();
        }
    }
}