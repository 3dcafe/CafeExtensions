using System.Security.Cryptography;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace CafeExtensions.SimpleModels
{
    /// <summary>
    /// Авторизация через эпл
    /// </summary>
    public class AppleAccount3RdParty : Account3rdParty
    {
        /// <summary>
        /// Адрес проверки токена
        /// </summary>
        protected override string ValidationUrl { get { return "https://appleid.apple.com/auth/keys"; } }
        /// <summary>
        /// Проверить токен
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public override async Task<AccoResponse?> ValidateAccount(string accessToken)
        {
            var keys = await GetJwksAsync();
            var tokens = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            if (keys == null) return null;
            var jwks = keys.Where(n => n.Kid == tokens.Header.Kid && n.Alg == tokens.Header.Alg).FirstOrDefault();
            if (jwks == null)
                return null;

            var isValidToken = ValidateToken(accessToken, jwks, tokens);
            if (!isValidToken)
                return null;

            if (tokens != null && tokens.Payload != null && tokens.Payload.ContainsKey("email"))
            {
                return new AccoResponse()
                {
                    Social = new AccountSocialInfo()
                    {
                        Email = tokens!.Payload["email"]!.ToString()!,
                        ExternalId = tokens.Payload.Sub,
                        Id = tokens.Id
                    }
                };
            }
            return null;
        }

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

        private bool ValidateToken(string accessToken, AppleJwksModel jwks, JwtSecurityToken tokens)
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

            if (tokens.Payload.Iss != "https://appleid.apple.com") return false;
            if (!tokens.Payload.Exp.HasValue || tokens.Payload.Exp < DateTimeOffset.UtcNow.ToUnixTimeSeconds()) return false;

            return true;
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
