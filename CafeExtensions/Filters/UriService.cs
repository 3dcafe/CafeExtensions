using CafeExtensions.Extensions;

namespace CafeExtensions.Filters
{
    /// <summary>
    /// Service for generating URLs for pagination.
    /// </summary>
    public class UriService : IUriService
    {
        private readonly string _baseUri;
        /// <summary>
        /// Initializes a new instance of the UriService class.
        /// </summary>
        /// <param name="baseUri">The base URI for the service.</param>
        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }
        /// <summary>
        /// Gets the URI for pagination.
        /// </summary>
        /// <param name="filter">Pagination filter.</param>
        /// <param name="route">Route for the URI.</param>
        /// <returns>The URI for pagination.</returns>
        public Uri GetPageUri(PaginationFilter filter, string route)
        {
            // Create the endpoint URI by concatenating the base URI and the route.
            var endpointUri = new Uri(string.Concat(_baseUri, route));

            // Modify the URI by adding query parameters for page number and page size.
            var modifiedUri = UrlExtensions.AddQueryString(
                endpointUri.ToString(),
                new Dictionary<string, string>
                {
                    { "pageNumber", filter.PageNumber.ToString() },
                    { "pageSize", filter.PageSize.ToString() }
                });

            // Return the modified URI.
            return new Uri(modifiedUri);
        }
    }
}