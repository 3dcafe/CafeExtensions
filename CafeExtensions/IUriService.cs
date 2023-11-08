using CafeExtensions.Filters;

namespace CafeExtensions
{
    /// <summary>
    /// In this interface, we have a function definition that takes in the pagination Filter and a route string (api/customer).
    /// PS, we will have to dynamically build this route string,
    /// as we are building it in a way that it can be used by any controller (Product, Invoice, Suppliers, etc etc) 
    /// and on any host (localhost, api.com, etc). 
    /// Clean and Efficient Coding is what you have to concentrate on! 😀
    /// </summary>
    public interface IUriService
    {
        /// <summary>
        /// Base method Get Page Uri
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="route"></param>
        /// <returns></returns>
        public Uri GetPageUri(PaginationFilter filter, string route);
    }

}
