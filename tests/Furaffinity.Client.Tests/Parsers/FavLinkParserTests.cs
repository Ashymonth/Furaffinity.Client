using System.IO;
using Furaffinity.Client.Parsers;
using Xunit;

namespace Furaffinity.Client.Tests.Parsers;

public class FavLinkParserTests
{
    [Fact]
    public void GetFavLinkTest_Should_ReturnFavLink()
    {
        const string path = "TestData\\ImagePage.html";

        var page = File.ReadAllText(path);

        var parser = new FavLinkParser();

        var actual = parser.GetFavLink(page);

        Assert.Equal("/fav/37238975/?key=9c2e30fdc83bbf0b9f855a97ecb2ec7f9cfc51ac", actual);
    }
}