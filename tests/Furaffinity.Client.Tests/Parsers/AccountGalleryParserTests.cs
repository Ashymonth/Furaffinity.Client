using System.IO;
using Furaffinity.Client.Parsers;
using Xunit;

namespace Furaffinity.Client.Tests.Parsers;

public class AccountGalleryParserTests
{
    [Fact]
    public void GetFileIdTest()
    {
        const string testSubmissionId = "37238975";
        const string path = "TestData\\AccountGalleryPage.html";

        var page = File.ReadAllText(path);

        var parser = new AccountGalleryParser();

        var actual = parser.GetSubmissionId(page);

        Assert.Equal(testSubmissionId, actual);
    }
}