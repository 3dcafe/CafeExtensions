using System.Text;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CafeExtensions.Helpers;
public class JwtHelper
{
    Dictionary<string, object> Payload;
    Dictionary<string, object> Header;
    public ClaimsPrincipal ReadJwtToken(string token)
    {
        try
        {
            string[] tokenParts = token.Split('.');
            if (tokenParts.Length != 3)
            {
                throw new ArgumentException("Invalid JWT format.");
            }

            string headerJson = Base64UrlDecode(tokenParts[0]);
            string payloadJson = Base64UrlDecode(tokenParts[1]);

            var claims = new List<Claim>();
            Payload = ParseJsonClaims(payloadJson);
            Header = ParseJsonClaims(headerJson);

            claims.AddRange(ParseJwtClaims(Payload));
            claims.AddRange(ParseJwtClaims(Header));

            var identity = new ClaimsIdentity(claims, "Custom");
            return new ClaimsPrincipal(identity);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error reading JWT token.", ex);
        }
    }

    Dictionary<string,object> ParseJsonClaims(string json) 
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };

        return JsonSerializer.Deserialize<Dictionary<string, object>>(json, options);
    }

    private string Base64UrlDecode(string base64Url)
    {
        string base64 = base64Url.Replace('-', '+').Replace('_', '/');
        while (base64.Length % 4 != 0)
        {
            base64 += '=';
        }
        return Encoding.UTF8.GetString(Convert.FromBase64String(base64));
    }

    private IEnumerable<Claim> ParseJwtClaims(Dictionary<string,object> data)
    {
        foreach (var kvp in data)
        {
            yield return new Claim(kvp.Key, kvp.Value?.ToString());
        }
    }
}
