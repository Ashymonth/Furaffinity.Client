using Furaffinity.Client.Exceptions;
using HtmlAgilityPack;

namespace Furaffinity.Client.Parsers;

internal class FormKeyParser
{
    public string GetFormKey(string page)
    {
        ErrorParser.ValidatePage(page);
        
        var document = new HtmlDocument();
        document.LoadHtml(page);

        var formKey = document.GetElementbyId("myform")
            .Descendants("input")
            .Skip(1)
            .FirstOrDefault()
            ?.GetAttributeValue("value", string.Empty);

        if (formKey == "0")
        {
            formKey = document.GetElementbyId("myform")
                .Descendants("input")
                .FirstOrDefault()
                ?.GetAttributeValue("value", string.Empty);
        }

        return formKey ?? throw new FuraffinityException("Unable to get form key");
    }
}