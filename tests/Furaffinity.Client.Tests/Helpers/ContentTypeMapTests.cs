using System;
using Furaffinity.Client.Helpers;
using Xunit;

namespace Furaffinity.Client.Tests.Helpers;

public class ContentTypeMapTests
{
    [Theory]
    [ClassData(typeof(ContentTypeTestData))]
    public void GetContentTypeByFileExtensionTest_Should_Match_With_All_Keys(string fileExtension, string contentType)
    {
        var result = ContentTypeMap.GetContentTypeByFileExtension(fileExtension);

        Assert.Equal(contentType, result);
    }

    [Fact]
    public void GetContentTypeByFileExtensionTest_Should_Throw_Exception()
    {
        Assert.Throws<ArgumentException>(() => ContentTypeMap.GetContentTypeByFileExtension("asd"));
    }
}

public class ContentTypeTestData : TheoryData<string, string>
{
    public ContentTypeTestData()
    {
        Add(".png", "image/png");
        Add(".jpg", "image/jpeg");
        Add(".jpeg", "image/jpeg");
        Add(".gif", "image/gif");

        Add(".txt", "text/plain");
        Add(".doc", "application/octet-stream");
        Add(".docx", "application/octet-stream");
        Add(".odt", "application/octet-stream");
        Add(".rtf", "application/octet-stream");
        Add(".pdf", "application/pdf");

        Add(".mp3", "audio/mpeg");
        Add(".wav", "audio/wav");
        Add(".mid", "audio/mid");
    }
}