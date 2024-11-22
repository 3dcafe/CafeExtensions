using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace CafeExtensions.Repositories;
/// <summary>
/// A repository class for making simple HTTP requests with optional basic authentication and custom timeout handling.
/// </summary>
public class HttpSimpleClientRepository : IDisposable
{
    private string? Login { get; set; }
    private string? Password { get; set; }
    private TimeSpan? Timeout { get; set; }
    private Dictionary<string, string> _headers = new();

    /// <summary>
    /// Default constructor.
    /// </summary>
    public HttpSimpleClientRepository() { }

    /// <summary>
    /// Constructor with basic authorization credentials.
    /// </summary>
    /// <param name="Login">Login for basic authentication.</param>
    /// <param name="Password">Password for basic authentication.</param>
    public HttpSimpleClientRepository(string Login, string Password)
    {
        this.Login = Login;
        this.Password = Password;
    }

    /// <summary>
    /// Constructor with basic authorization and a custom timeout.
    /// </summary>
    /// <param name="Login">Login for basic authentication.</param>
    /// <param name="Password">Password for basic authentication.</param>
    /// <param name="timeout">Custom timeout for HTTP requests.</param>
    public HttpSimpleClientRepository(string Login, string Password, TimeSpan timeout)
    {
        this.Login = Login;
        this.Password = Password;
        Timeout = timeout;
    }

    /// <summary>
    /// Constructor with a custom timeout only.
    /// </summary>
    /// <param name="timeout">Custom timeout for HTTP requests.</param>
    public HttpSimpleClientRepository(TimeSpan timeout)
    {
        Timeout = timeout;
    }

    /// <summary>
    /// Sends an HTTP GET request and returns the deserialized response.
    /// </summary>
    /// <typeparam name="T">Type of the expected response.</typeparam>
    /// <param name="url">The URL to send the request to.</param>
    /// <returns>A response wrapped in <see cref="RestClientResponse{T}"/>.</returns>
    public async Task<RestClientResponse<T>> GetAsync<T>(string url)
    {
        using var httpClient = CreateHttpClient();

        HttpResponseMessage response = await httpClient.GetAsync(url);
        return await CreateRestClientResponse<T>(response);
    }

    /// <summary>
    /// Sends an HTTP GET request without expecting a response body.
    /// </summary>
    /// <param name="url">The URL to send the request to.</param>
    public async Task GetUpdateAsync(string url)
    {
        using var httpClient = CreateHttpClient();
        await httpClient.GetAsync(url);
    }

    /// <summary>
    /// Sends an HTTP POST request with an object as JSON content and deserializes the response.
    /// </summary>
    /// <typeparam name="T">Type of the expected response.</typeparam>
    /// <param name="url">The URL to send the request to.</param>
    /// <param name="objectToPost">The object to send as JSON content.</param>
    /// <param name="formUrlencoded">Indicates if the content should be sent as form-urlencoded.</param>
    /// <returns>A response wrapped in <see cref="RestClientResponse{T}"/>.</returns>
    public async Task<RestClientResponse<T>?> PostAsync<T>(string url, object objectToPost, bool formUrlencoded = false)
    {
        using var httpClient = CreateHttpClient();

        HttpResponseMessage response;
        if (formUrlencoded)
        {
            var dict = (Dictionary<string, string>)objectToPost;
            response = await httpClient.PostAsync(url, new FormUrlEncodedContent(dict));
        }
        else
        {
            response = await httpClient.PostAsJsonAsync(url, objectToPost);
        }

        return await CreateRestClientResponse<T>(response);
    }

    /// <summary>
    /// Sets custom headers for HTTP requests.
    /// </summary>
    /// <param name="dictionary">A dictionary of headers to set.</param>
    public void SetHeaders(Dictionary<string, string> dictionary)
    {
        _headers.Clear();
        foreach (var item in dictionary)
        {
            _headers.Add(item.Key, item.Value);
        }
    }

    /// <summary>
    /// Sets the OrganizationId header for HTTP requests.
    /// </summary>
    /// <param name="organizationId">The OrganizationId value to set.</param>
    public void SetHeaders(string organizationId)
    {
        _headers = new Dictionary<string, string>
            {
                { "OrganizationId", organizationId }
            };
    }

    /// <summary>
    /// Disposes resources used by the class.
    /// </summary>
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Creates and configures an <see cref="HttpClient"/> instance.
    /// </summary>
    /// <returns>A configured <see cref="HttpClient"/>.</returns>
    private HttpClient CreateHttpClient()
    {
        var httpClient = new HttpClient();
        if (Timeout.HasValue)
        {
            httpClient.Timeout = Timeout.Value;
        }

        if (_headers.Count > 0)
        {
            httpClient.DefaultRequestHeaders.Clear();
            foreach (var header in _headers)
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        if (!string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password))
        {
            var authenticationString = $"{Login}:{Password}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.UTF8.GetBytes(authenticationString));
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + base64EncodedAuthenticationString);
        }

        return httpClient;
    }

    /// <summary>
    /// Creates a <see cref="RestClientResponse{T}"/> from an <see cref="HttpResponseMessage"/>.
    /// </summary>
    /// <typeparam name="T">Type of the expected response.</typeparam>
    /// <param name="response">The HTTP response message.</param>
    /// <returns>A <see cref="RestClientResponse{T}"/> instance.</returns>
    private async Task<RestClientResponse<T>> CreateRestClientResponse<T>(HttpResponseMessage response)
    {
        var restClientResponse = new RestClientResponse<T>
        {
            StatusCode = (int)response.StatusCode,
            StatusName = response.StatusCode,
            Message = response.ReasonPhrase,
            Response = response.StatusCode == HttpStatusCode.OK
                ? await response.Content.ReadFromJsonAsync<T>()
                : default
        };

        return restClientResponse;
    }
}