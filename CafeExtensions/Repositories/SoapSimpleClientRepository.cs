using System.Net;
using System.Net.Http;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using System.Net.Http.Json;

namespace CafeExtensions.Repositories
{
    public class SoapSimpleClientRepository
    {
        private string? Login { get; set; }
        private string? Password { get; set; }
        /// <summary>
        /// Header custom
        /// </summary>
        private Dictionary<string, string> _headers = new();
        /// <summary>
        /// Default contructor
        /// </summary>
        public SoapSimpleClientRepository() { }
        /// <summary>
        /// Contructor with basic autrization login
        /// </summary>
        /// <param name="Login"></param>
        /// <param name="Password"></param>
        public SoapSimpleClientRepository(string Login, string Password)
        {
            this.Login = Login;
            this.Password = Password;
        }

        public async Task<RestClientResponse<string>?> ExecuteAsync(string url, string body)
        {
            using (HttpContent content = new StringContent(body, Encoding.UTF8, "application/xml"))
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url))
            using (var httpClient = new HttpClient())
            {
                if (_headers.Count > 0)
                {
                    httpClient.DefaultRequestHeaders.Clear();
                    foreach (var item in _headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }

                if (Login?.Length > 0 && Password?.Length > 0)
                {
                    var authenticationString = $"{Login}:{Password}";
                    var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authenticationString));

                    // Remove additional Authorization header from above
                    if (httpClient.DefaultRequestHeaders.Contains("Authorization")) httpClient.DefaultRequestHeaders.Remove("Authorization");

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + base64EncodedAuthenticationString);
                }

                request.Headers.Add("SOAPAction", "#POST");
                request.Content = content;

                using (HttpResponseMessage response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    var data = await response.Content.ReadAsStringAsync();
                    try
                    {
                        var restClientResponse = new RestClientResponse<string>
                        {
                            StatusCode = (int)response.StatusCode,
                            StatusName = response.StatusCode,
                            Message = response.ReasonPhrase,
                            Response = response.StatusCode != HttpStatusCode.OK ?
                                default :
                                data
                        };
                        return restClientResponse;
                    }
                    catch (Exception)
                    {
                        return default;
                    }
                }
            }
        }
    }
}
