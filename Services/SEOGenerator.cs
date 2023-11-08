using System.Text.RegularExpressions;


namespace CafeExtensions.Services;
/// <summary>
/// lass is a utility class designed to generate SEO (Search Engine Optimization) 
/// keywords from a given text. SEO keywords play a crucial role in improving a web page's visibility in search engine results.
/// This class provides a method for generating relevant keywords while considering stop words and limiting the number of generated keywords 
/// to ensure SEO compliance.
/// </summary>
public static class SEOGenerator
{
    /// <summary>
    /// Method is designed to generate a meta-description based on the input title and body text.
    /// It takes three parameters: title, which represents the page's title, body, which is the content of the page, and maxDescriptionLength, indicating the maximum length for the meta-description.
    /// The method combines the title and body text, removes excess white spaces and line breaks, and ensures that the resulting description adheres to the specified maximum length.
    /// It is a valuable tool for creating concise and SEO-optimized meta-descriptions for web pages, enhancing their visibility and ranking in search engine results.
    /// By utilizing this method, web developers and SEO practitioners can efficiently craft meta-descriptions that provide an accurate and engaging summary of a web page's content while complying with SEO best practices.
    /// </summary>
    /// <param name="title"></param>
    /// <param name="body"></param>
    /// <param name="maxDescriptionLength"></param>
    /// <returns></returns>
    public static string GenerateDescription(string title, string body, int maxDescriptionLength = 90)
    {
        string fullText = title + " " + body;
        fullText = Regex.Replace(fullText, @"\s+", " ").Trim();

        if (fullText.Length > maxDescriptionLength)
            fullText = fullText.Substring(0, maxDescriptionLength);
        return fullText;
    }
    /// <summary>
    /// method is a key function within the SEOKeywordGenerator class.
    /// It takes two input parameters, title and body, representing the title and body text of a web page.
    /// Additionally, it accepts an optional parameter, maxKeywordCount, which specifies the maximum number of keywords to generate. 
    /// This method processes the input text, splits it into words, converts them to lowercase, and removes common stop words.
    /// It then returns a list of unique keywords, ensuring their relevance to the content.
    /// </summary>
    /// <param name="title"></param>
    /// <param name="body"></param>
    /// <param name="maxKeywordCount"></param>
    /// <returns></returns>
    public static List<string> GenerateKeywords(string title, string body, int maxKeywordCount = 8)
    {
        List<string> stopWords = new List<string>
        {
            "и", "в", "на", "не", "с", "по", "для", "как", "о", "от"
        };
        string text = title + " " + body;
        string[] words = Regex.Split(text, @"\W+");
        List<string> filteredWords = words
            .Select(word => word.ToLower())
            .Where(word => !stopWords.Contains(word))
            .Distinct()
            .ToList();

        if (filteredWords.Count > maxKeywordCount)
            filteredWords = filteredWords.Take(maxKeywordCount).ToList();
        return filteredWords;
    }
}