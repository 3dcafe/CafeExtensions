using System.Security.Cryptography;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;
using CafeExtensions.Helpers;
using System.Security.Claims;
using CafeExtensions.Extensions;

namespace CafeExtensions.SimpleModels
{
    /// <summary>
    /// Authorization through Apple
    /// </summary>
    public class AppleAccount3RdParty : Account3rdParty
    {
        /// <summary>
        /// URL for token validation
        /// </summary>
        protected override string ValidationUrl { get { return "https://appleid.apple.com/auth/keys"; } }

        /// <summary>
        /// Validate the account using the provided access token
        /// </summary>
        /// <param name="accessToken">Access token to validate</param>
        /// <returns>AccoResponse or null if validation fails</returns>
        public override async Task<AccoResponse?> ValidateAccount(string accessToken)
        {
            // Retrieve JSON Web Key Set (JWKS)
            var keys = await GetJwksAsync();
            var tokens = new JwtHelper().ReadJwtToken(accessToken);

            if (keys == null)
                return null;

            // Find a JWKS entry that matches the 'kid' and 'alg' claims from the token
            var jwks = keys.FirstOrDefault(n => n.Kid == tokens.Claims?.FirstOrDefault(x => x.Type == "kid")?.Value
                                                && n.Alg == tokens.Claims?.FirstOrDefault(x => x.Type == "alg")?.Value);

            if (jwks == null)
                return null;

            // Validate the token's signature
            var isValidToken = ValidateToken(accessToken, jwks, tokens);

            if (!isValidToken)
                return null;

            // Extract user information from the token claims
            if (tokens != null && tokens.Claims.Any(x => x.Type == "email"))
            {
                return new AccoResponse()
                {
                    Social = new AccountSocialInfo()
                    {
                        Email = tokens.Claims?.FirstOrDefault(x => x.Type == "email")?.Value ?? string.Empty,
                        ExternalId = tokens.Claims?.FirstOrDefault(x => x.Type == "sub")?.Value ?? string.Empty,
                        Id = tokens.Claims?.FirstOrDefault(x => x.Type == "id")?.Value ?? string.Empty
                    }
                };
            }

            return null;
        }

        /// <summary>
        /// Asynchronously retrieves JWKS from the Apple endpoint
        /// </summary>
        /// <returns>Array of AppleJwksModel or null if retrieval fails</returns>
        private async Task<AppleJwksModel[]?> GetJwksAsync()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("https://appleid.apple.com/auth/keys");

                if (!response.IsSuccessStatusCode)
                {
                    return Array.Empty<AppleJwksModel>();
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                var jwks = JsonSerializer.Deserialize<AppleJwksResponseModel>(responseContent, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                });

                if (jwks == null)
                    return null;

                return jwks.Keys;
            }
        }

        /// <summary>
        /// Validates the token's signature and claims
        /// </summary>
        /// <param name="accessToken">Access token to validate</param>
        /// <param name="jwks">Apple JWKS model</param>
        /// <param name="claims">Token claims</param>
        /// <returns>True if the token is valid, otherwise false</returns>
        private bool ValidateToken(string accessToken, AppleJwksModel jwks, ClaimsPrincipal claims)
        {
            var tokenParts = accessToken.Split('.');
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(new RSAParameters()
            {
                Modulus = jwks?.N?.ConvertToBase64(),
                Exponent = jwks?.E?.ConvertToBase64(),
            });

            var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes($"{tokenParts[0]}.{tokenParts[1]}"));

            var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
            rsaDeformatter.SetHashAlgorithm("SHA256");

            var isValidSignature = rsaDeformatter.VerifySignature(hash, tokenParts[2].ConvertToBase64());

            if (!isValidSignature)
                return false;

            var issClaim = claims.Claims.FirstOrDefault(x => x.Type == "iss");

            if (issClaim?.Value != "https://appleid.apple.com")
                return false;

            if (long.TryParse(claims.Claims.FirstOrDefault(x => x.Type == "exp")?.Value, out long exp))
                return exp >= DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            return false;
        }
    }
    /// <summary>
    /// Model anwer
    /// </summary>
    internal class AppleJwksModel
    {
        /// <summary>
        /// Kty
        /// </summary>
        [JsonPropertyName("kty")]
        public string? Kty { get; set; }
        /// <summary>
        /// Kid
        /// </summary>
        [JsonPropertyName("kid")]
        public string? Kid { get; set; }
        /// <summary>
        /// Use
        /// </summary>
        [JsonPropertyName("use")]
        public string? Use { get; set; }
        /// <summary>
        /// Alg
        /// </summary>
        [JsonPropertyName("alg")]
        public string? Alg { get; set; }
        /// <summary>
        /// N
        /// </summary>
        [JsonPropertyName("n")]
        public string? N { get; set; }
        /// <summary>
        /// E
        /// </summary>
        [JsonPropertyName("e")]
        public string? E { get; set; }
    }
    /// <summary>
    /// JWT Token Response Model
    /// </summary>
    internal class AppleJwksResponseModel
    {
        /// <summary>
        /// Keys models
        /// </summary>
        [JsonPropertyName("keys")]
        public AppleJwksModel[]? Keys { get; set; }
    }
}
