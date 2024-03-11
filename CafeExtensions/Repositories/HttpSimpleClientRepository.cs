using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace CafeExtensions.Repositories
{
    public class HttpSimpleClientRepository
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
        public HttpSimpleClientRepository() { }
        /// <summary>
        /// Contructor with basic autrization login
        /// </summary>
        /// <param name="Login"></param>
        /// <param name="Password"></param>
        public HttpSimpleClientRepository(string Login, string Password)
        {
            this.Login = Login;
            this.Password = Password;
        }
        /// <summary>
        /// Get request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<RestClientResponse<T>> GetAsync<T>(string url)
        {
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

                var response = await httpClient.GetAsync(url);
                var restClientResponse = new RestClientResponse<T>
                {
                    StatusCode = (int)response.StatusCode,
                    StatusName = response.StatusCode,
                    Message = response.ReasonPhrase,
                    Response = response.StatusCode != HttpStatusCode.OK ?
                        default :
                        await response.Content.ReadFromJsonAsync<T>()
                };
                return restClientResponse;
            }
        }

        /// <summary>
        /// Get request without content
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task GetUpdateAsync(string url)
        {
            using (var httpClient = new HttpClient())
            {
                if (Login?.Length > 0 && Password?.Length > 0)
                {
                    var authenticationString = $"{Login}:{Password}";
                    var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authenticationString));
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + base64EncodedAuthenticationString);
                }
                var response = await httpClient.GetAsync(url);
            }
        }

        /// <summary>
        /// Get file from URL
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<byte[]?> GetFile(string url)
        {
            using var httpClient = new HttpClient();
            try
            {
                using var stream = await httpClient.GetStreamAsync(url);
                byte[] buffer = new byte[16 * 1024];
                using MemoryStream ms = new MemoryStream();
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка получения файла: - " + url + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Post request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="objectToPost"></param>
        /// <returns></returns>
        public async Task<RestClientResponse<T>?> PostAsync<T>(string url, object objectToPost, bool formUrlencoded = false)
        {
            Console.WriteLine($"post->{url}");
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
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + base64EncodedAuthenticationString);
                }

                if (formUrlencoded)
                {
                    var dict = (Dictionary<string, string>)objectToPost;
                    var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(dict) };
                    var response = await httpClient.SendAsync(req);
                    try
                    {
                        var restClientResponse = new RestClientResponse<T>
                        {
                            StatusCode = (int)response.StatusCode,
                            StatusName = response.StatusCode,
                            Message = response.ReasonPhrase,
                            Response = response.StatusCode != HttpStatusCode.OK ?
                                default :
                                await response.Content.ReadFromJsonAsync<T>()
                        };
                        return restClientResponse;
                    }
                    catch (Exception)
                    {
                        return default;
                    }
                }
                else
                {
                    var response = await httpClient.PostAsJsonAsync(url, objectToPost);
#if DEBUG
                    string jsonTest = JsonSerializer.Serialize(objectToPost);
                    string jsonResponse = await response.Content.ReadAsStringAsync();
#endif
                    try
                    {
                        var restClientResponse = new RestClientResponse<T>
                        {
                            StatusCode = (int)response.StatusCode,
                            StatusName = response.StatusCode,
                            Message = response.ReasonPhrase,
                            Response = response.StatusCode != HttpStatusCode.OK ?
                                default :
                                await response.Content.ReadFromJsonAsync<T>()
                        };
                        return restClientResponse;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        return default;
                    }
                }
            }
        }


        /// <summary>
        /// Post request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="objectToPost"></param>
        /// <returns></returns>
        public async Task<bool> PostAsyncWithoutAnswer(string url, object objectToPost)
        {
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
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + base64EncodedAuthenticationString);
                }

                var response = await httpClient.PostAsJsonAsync(url, objectToPost);
                try
                {
                    return response.StatusCode != HttpStatusCode.OK ? false : true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return false;
                }
            }
        }

        /// <summary>
        /// Post request for get stream file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="objectToPost"></param>
        /// <returns></returns>
        public async Task<Stream?> PostGetStreamAsync(string url, object objectToPost)
        {
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
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + base64EncodedAuthenticationString);
                }

                var response = await httpClient.PostAsJsonAsync(url, objectToPost);
                try
                {
                    return await response.Content.ReadAsStreamAsync();
                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString());
                    return null;
                }
            }
        }



        public async Task<RestClientResponse<T>> FileUplopadAsync<T>(string url, string FileName, byte[] fileData)
        {
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
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + base64EncodedAuthenticationString);
                }

                using (var multipartFormContent = new MultipartFormDataContent())
                {
                    var byteContent = new ByteArrayContent(fileData);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                    multipartFormContent.Add(byteContent, name: "uploadedFiles", fileName: FileName);

                    var response = await httpClient.PostAsync(url, multipartFormContent);
                    try
                    {
                        var restClientResponse = new RestClientResponse<T>
                        {
                            StatusCode = (int)response.StatusCode,
                            StatusName = response.StatusCode,
                            Message = response.ReasonPhrase,
                            Response = response.StatusCode != HttpStatusCode.OK ?
                            default :
                            await response.Content.ReadFromJsonAsync<T>()
                        };
                        return restClientResponse;
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.ToString());
                        var restClientResponse = new RestClientResponse<T>
                        {
                            StatusCode = (int)response.StatusCode,
                            StatusName = response.StatusCode,
                            Message = response.ReasonPhrase,
                            Response = default
                        };
                        return restClientResponse;
                    }
                }
            }
        }


        public void SetHeaders(Dictionary<string, string> dictionary)
        {
            this._headers.Clear();
            foreach (var item in dictionary)
            {
                _headers.Add(item.Key, item.Value);
            }
        }
        /// <summary>
        /// Set headers with OrganizationId key
        /// </summary>
        /// <param name="organizationId"></param>
        public void SetHeaders(string organizationId)
        {
            this._headers =
                new Dictionary<string, string>()
                {
                    { "OrganizationId" , organizationId }
                };
        }
    }
}
