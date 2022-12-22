using System.IO;
using Furaffinity.Client.Parsers;
using Xunit;

namespace Furaffinity.Client.Tests.Parsers;

public class FormKeyParserTests
{
    [Theory]
    [InlineData("UploadSubmissionPage.html", "ff72a266ce57b1cc4aae8dc8e2941e4524284a88")]
    [InlineData("FinalizeSubmissionPage.html", "b2d9e4421cdc0f240828af4acfd9a81addd482d3")]
    public void GetFormKeyTest_Should__Return_Form_Key_From_Upload_Page(string pagePath, string expectedFormKey)
    {
        var pathToPage = $"TestData\\{pagePath}";

        var page = File.ReadAllText(pathToPage);

        var parser = new FormKeyParser();

        var actual = parser.GetFormKey(page);

        Assert.Equal(expectedFormKey, actual);
    }
}