using System.IO;
using System.Threading.Tasks;
using Furaffinity.Client.Parsers;
using Xunit;

namespace Furaffinity.Client.Tests.Parsers;

public class ConfirmDeleteSubmissionFormKeyParserTests
{
    [Fact]
    public async Task GetFormKeyTest_Should_Return_Form_Key()
    {
        const string expectedFormKey = "2c5b025c027b46b43e419d205f60fd5b4c29968c";
        const string pathToPage = "TestData\\ConfirmDeletePage.html";

        var page = await  File.ReadAllTextAsync(pathToPage);

        var parser = new ConfirmDeleteSubmissionFormKeyParser();

        var actual = parser.GetFormKey(page);

        Assert.Equal(expectedFormKey, actual);
    }
}