using Microsoft.AspNetCore.WebUtilities;

namespace CafeExtensions.Filters
{
    /// <summary>
    /// Сервис генерации url для пагинации
    /// </summary>
    public class UriService : IUriService
    {
        private readonly string _baseUri;
        /// <summary>
        /// Базовый констурктор
        /// </summary>
        /// <param name="baseUri"></param>
        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }
        /// <summary>
        /// Метод получения url для пагинации
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="route"></param>
        /// <returns></returns>
        public Uri GetPageUri(PaginationFilter filter, string route)
        {
            var _enpointUri = new Uri(string.Concat(_baseUri, route));
            var modifiedUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "pageNumber", filter.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", filter.PageSize.ToString());
            return new Uri(modifiedUri);
        }
    }
}