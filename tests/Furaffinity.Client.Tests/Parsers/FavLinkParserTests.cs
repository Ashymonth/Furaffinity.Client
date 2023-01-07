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

        Assert.Equal("/fav/50515362/?key=dc84d83f2c4cfdda1ea6cdc554eb638aafd186e3", actual);
    }
}