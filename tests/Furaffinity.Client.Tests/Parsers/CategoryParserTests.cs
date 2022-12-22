using System.IO;
using Furaffinity.Client.Parsers;
using Xunit;

namespace Furaffinity.Client.Tests.Parsers;

public class CategoryParserTests
{
    [Fact]
    public void Test()
    {
        const string path = "TestData\\CategoryPage.html";

        var page = File.ReadAllText(path);

        var parser = new CategoryParser();

        var actual = parser.GetCategories(page);

        Assert.True(actual.Count == 4);
    }
}