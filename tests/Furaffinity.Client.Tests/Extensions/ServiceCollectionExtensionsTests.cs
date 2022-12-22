using Furaffinity.Client.Extensions;
using Furaffinity.Client.Resources;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Furaffinity.Client.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddFuraffinityClientTests_Should_Return_Services()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddFuraffinityClient();

        var provider = serviceCollection.BuildServiceProvider();

        provider.GetRequiredService<IFuraffinityClient>();
        provider.GetRequiredService<IAccountResource>();
        provider.GetRequiredService<ISubmissionResource>();
        provider.GetRequiredService<IGalleryResource>();
    }
}