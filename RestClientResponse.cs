using System.Net;

namespace CafeExtensions
{
    public class RestClientResponse<T>
    {
        /// <summary>
        /// Code answer out system
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// Name status answer 
        /// </summary>
        public HttpStatusCode StatusName { get; set; }
        /// <summary>
        /// Message answer out system
        /// </summary>
        public string? Message { get; set; }
        /// <summary>
        /// Response from out system
        /// </summary>
        public T? Response { get; set; }
    }
}
