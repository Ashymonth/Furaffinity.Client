using System.IO;
using Furaffinity.Client.Parsers;
using Xunit;

namespace Furaffinity.Client.Tests.Parsers;

public class SubmissionIdParserTests
{
    [Fact]
    public void GetSubmissionIdTest()
    {
        const string path = "TestData\\SubmissionPageAfterUpload.html";

        var page = File.ReadAllText(path);

        var parser = new SubmissionIdParser();

        var actual = parser.GetSubmissionId(page);

        Assert.Equal("50515362", actual);
    }
}