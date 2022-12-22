using System.IO;
using Furaffinity.Client.Parsers;
using Xunit;

namespace Furaffinity.Client.Tests.Parsers;

public class AccountLinkBrowsePageParserTests
{
    [Fact]
    public void GetUserLinkTest()
    {
        const string path = "TestData\\CategoryPage.html";

        var page = File.ReadAllText(path);

        var parser = new AccountLinkBrowsePageParser();

        var actual = parser.GetAccountId(page);

        Assert.Equal("/user/sweetyfoxy656/", actual);
    }
}