using System.IO;
using System.Threading.Tasks;
using Furaffinity.Client.Exceptions;
using Furaffinity.Client.Parsers.Errors;
using Xunit;

namespace Furaffinity.Client.Tests.Parsers.Errors;

public class UnauthorizedPageParserTests
{
    [Fact]
    public async Task ValidatePage_Should_Throw_Exception()
    {
        const string path = "TestData\\UnauthorizedPage.html";

        var unauthorizedPage = await File.ReadAllTextAsync(path);
        
        var parser = new UnauthorizedPageParser();

        Assert.Throws<AuthorizationCookieException>(() => parser.ValidatePage(unauthorizedPage));
    }

    [Fact]
    public async Task ValidatePage_Should_Not_Throw_Exception()
    {
        const string path = "TestData\\AccountSubmisssionsManagePage.html";

        var unauthorizedPage = await File.ReadAllTextAsync(path);
        
        var parser = new UnauthorizedPageParser();

        parser.ValidatePage(unauthorizedPage);
    }
}