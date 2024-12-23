using System.Text;

namespace CafeExtensions.Repositories;
/// <summary>
/// A repository class for making SOAP-based HTTP requests with optional basic authentication and custom timeout handling.
/// </summary>
public class SoapSimpleClientRepository : IDisposable
{
    private string? Login { get; set; } // Stores the login for basic authentication
    private string? Password { get; set; } // Stores the password for basic authentication
    private TimeSpan? Timeout { get; set; } // Optional timeout for HTTP requests
    private readonly Dictionary<string, string> _headers = new(); // Custom headers for HTTP requests

    /// <summary>
    /// Default constructor for initializing the repository without authentication or timeout.
    /// </summary>
    public SoapSimpleClientRepository() { }

    /// <summary>
    /// Constructor for initializing the repository with basic authentication credentials.
    /// </summary>
    /// <param name="Login">Login for basic authentication.</param>
    /// <param name="Password">Password for basic authentication.</param>
    public SoapSimpleClientRepository(string Login, string Password)
    {
        this.Login = Login;
        this.Password = Password;
    }

    /// <summary>
    /// Constructor for initializing the repository with basic authentication and a custom timeout.
    /// </summary>
    /// <param name="Login">Login for basic authentication.</param>
    /// <param name="Password">Password for basic authentication.</param>
    /// <param name="timeout">Custom timeout for HTTP requests.</param>
    public SoapSimpleClientRepository(string Login, string Password, TimeSpan timeout)
    {
        this.Login = Login;
        this.Password = Password;
        Timeout = timeout;
    }

    /// <summary>
    /// Constructor for initializing the repository with a custom timeout only.
    /// </summary>
    /// <param name="timeout">Custom timeout for HTTP requests.</param>
    public SoapSimpleClientRepository(TimeSpan timeout)
    {
        Timeout = timeout;
    }

    /// <summary>
    /// Executes a SOAP request with the specified URL and body.
    /// </summary>
    /// <param name="url">The endpoint URL for the SOAP request.</param>
    /// <param name="body">The SOAP message body in XML format.</param>
    /// <returns>A RestClientResponse object containing the HTTP response details, or null if an error occurs.</returns>
    public async Task<RestClientResponse<string>?> ExecuteAsync(string url, string body)
    {
        // Prepare the request content
        using HttpContent content = new StringContent(body, Encoding.UTF8, "application/xml");
        using HttpRequestMessage request = new(HttpMethod.Post, url);
        using var httpClient = new HttpClient();

        // Apply custom timeout if provided
        if (Timeout.HasValue)
            httpClient.Timeout = Timeout.Value;

        // Add custom headers to the request
        if (_headers.Count > 0)
        {
            httpClient.DefaultRequestHeaders.Clear();
            foreach (var item in _headers)
                httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
        }

        // Add basic authentication header if credentials are provided
        if (!string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password))
        {
            string authenticationString = $"{Login}:{Password}";
            string base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.UTF8.GetBytes(authenticationString));

            if (httpClient.DefaultRequestHeaders.Contains("Authorization"))
                httpClient.DefaultRequestHeaders.Remove("Authorization");

            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + base64EncodedAuthenticationString);
        }

        // Set required SOAP-specific headers
        request.Headers.Add("SOAPAction", "#POST");
        request.Content = content;

        // Execute the HTTP request
        using HttpResponseMessage response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        string data = await response.Content.ReadAsStringAsync();

        // Parse and return the response
        return new RestClientResponse<string>
        {
            StatusCode = (int)response.StatusCode,
            StatusName = response.StatusCode,
            Message = response.ReasonPhrase,
            Response = response.IsSuccessStatusCode ? data : null
        };
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}