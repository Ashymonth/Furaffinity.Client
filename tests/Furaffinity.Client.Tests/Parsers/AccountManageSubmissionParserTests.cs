using System.IO;
using System.Threading.Tasks;
using Furaffinity.Client.Parsers;
using Xunit;

namespace Furaffinity.Client.Tests.Parsers;

public class AccountManageSubmissionParserTests
{
    [Fact]
    public async Task HasAnySubmissionsTest_Should_Return_True()
    {
        const string pathToPage = "TestData\\AccountSubmisssionsManagePage.html";

        var testPager = await File.ReadAllTextAsync(pathToPage);

        var parser = new AccountManageSubmissionParser();

        var actual = parser.HasAnySubmissions(testPager);

        Assert.True(actual);
    }
}