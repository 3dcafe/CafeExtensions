using System.Web;

namespace CafeExtensions.Extensions;

public static class UrlExtensions
{
    public static string AddQueryString(string url, Dictionary<string, string> parameters)
    {
        if (string.IsNullOrEmpty(url))
            throw new ArgumentNullException(nameof(url));
        if (parameters == null || parameters.Count == 0)
            return url;
        var uriBuilder = new UriBuilder(url);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        foreach (var parameter in parameters)
        {
            query[parameter.Key] = parameter.Value;
        }
        uriBuilder.Query = query.ToString();
        return uriBuilder.Uri.AbsoluteUri;
    }
}