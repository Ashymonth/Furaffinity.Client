using Furaffinity.Client.Contracts;
using HtmlAgilityPack;

namespace Furaffinity.Client.Parsers;

internal class CategoryParser
{
    public Dictionary<string, List<CategoryResponse>> GetCategories(string page)
    {
        var document = new HtmlDocument();
        document.LoadHtml(page);

        var result = new Dictionary<string, List<CategoryResponse>>(4);

        var categories = document.DocumentNode.Descendants("select");

        foreach (var category in categories)
        {
            var categoryName = category.ParentNode.Descendants("h3").First().InnerText;
            var subCategories = category.Descendants("optgroup").ToArray();

            result.Add(categoryName, new List<CategoryResponse>());
            if (!subCategories.Any())
            {
                result[categoryName].AddRange(GetCategoryGroup(category));
                continue;
            }

            foreach (var subCategory in subCategories)
            {
                var inner = new List<CategoryResponse>
                {
                    new()
                    {
                        Name = subCategory.GetAttributeValue("label", string.Empty),
                        Value = null
                    }
                };

                inner.AddRange(GetCategoryGroup(subCategory));

                result[categoryName].AddRange(inner);
            }
        }

        return result;
    }

    private static IEnumerable<CategoryResponse> GetCategoryGroup(HtmlNode node)
    {
        var options = node.Descendants("option");

        foreach (var option in options)
        {
            var categoryGroup = new CategoryResponse
            {
                Name = option.InnerText.Trim(),
                Value = option.GetAttributeValue("value", string.Empty),
            };

            yield return categoryGroup;
        }
    }
}