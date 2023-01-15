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

        Assert.Equal("/fav/50608865/?key=7a1e52fd72fdf54776c749bf47895a38b454b2c7", actual);
    }
}