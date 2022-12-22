using HtmlAgilityPack;

namespace Furaffinity.Client.Parsers.Errors;

internal interface IErrorParser
{
    void ValidatePage(string document);
}