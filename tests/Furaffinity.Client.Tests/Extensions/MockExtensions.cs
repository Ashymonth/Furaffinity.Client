using System;
using System.Net.Http;
using Moq;
using Moq.Contrib.HttpClient;

namespace Furaffinity.Client.Tests.Extensions;

public static class MockExtensions
{
    public static HttpClient CreateClientWithBaseUrl(this Mock<HttpMessageHandler> httpClientHandler)
    {
        var client = httpClientHandler.CreateClient();

        client.BaseAddress = new Uri(Constants.BaseUrl);

        return client;
    }
}