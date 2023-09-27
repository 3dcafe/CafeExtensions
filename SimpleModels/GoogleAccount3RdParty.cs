using System.Text.Json;
using System.Text.Json.Serialization;

namespace CafeExtensions.SimpleModels
{
    /// <summary>
    /// Google Service
    /// </summary>
    public class GoogleAccount3RdParty : Account3rdParty
    {
        /// <summary>
        /// Url validate
        /// </summary>
        protected override string ValidationUrl { get { return "https://www.googleapis.com/oauth2/v3/tokeninfo?id_token={0}"; } }
        /// <summary>
        /// Проверка токена
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public override async Task<AccoResponse?> ValidateAccount(string token)
        {
            var response = await GetResultAsync(token);
            if (response == null) return null;
            if (response.Message == null) return null;
            var googleApiTokenInfo = JsonSerializer.Deserialize<GoogleApiTokenInfo>(response.Message);
            if (googleApiTokenInfo == null) return null;
            var accoResponse = new AccoResponse();
            if (response.Code != System.Net.HttpStatusCode.OK)
                accoResponse.Error = new AccoResponseError { Message = googleApiTokenInfo.ErrorDescription, Code = response.Code.ToString() };
            else
                accoResponse.Social = new AccountSocialInfo { Email = googleApiTokenInfo.Email!, Name = googleApiTokenInfo.Name!, ExternalId = googleApiTokenInfo.Sub!, Provider = ProviderName.Google };
            return accoResponse;
        }
    }
    /// <summary>
    /// Токен авторизации через гугл
    /// </summary>
    public class GoogleApiTokenInfo
    {
        /// <summary>
        /// The Issuer Identifier for the Issuer of the response. Always https://accounts.google.com or accounts.google.com for Google ID tokens.
        /// </summary>
        [JsonPropertyName("iss")]
        public string? Iss { get; set; }
        /// <summary>
        /// Access token hash. Provides validation that the access token is tied to the identity token. If the ID token is issued with an access token in the server flow, this is always
        /// included. This can be used as an alternate mechanism to protect against cross-site request forgery attacks, but if you follow Step 1 and Step 3 it is not necessary to verify the 
        /// access token.
        /// </summary>
        [JsonPropertyName("at_hash")]
        public string? AtHash { get; set; }
        /// <summary>
        /// Identifies the audience that this ID token is intended for. It must be one of the OAuth 2.0 client IDs of your application.
        /// </summary>
        [JsonPropertyName("aud")]
        public string? Aud { get; set; }
        /// <summary>
        /// An identifier for the user, unique among all Google accounts and never reused. A Google account can have multiple emails at different points in time, but the sub value is never
        /// changed. Use sub within your application as the unique-identifier key for the user.
        /// </summary>
        [JsonPropertyName("sub")]
        public string? Sub { get; set; }
        /// <summary>
        /// True if the user e-mail address has been verified; otherwise false.
        /// </summary>
        [JsonPropertyName("email_verified")]
        public string? EmailVerified { get; set; }
        /// <summary>
        /// The client_id of the authorized presenter. This claim is only needed when the party requesting the ID token is not the same as the audience of the ID token. This may be the
        /// case at Google for hybrid apps where a web application and Android app have a different client_id but share the same project.
        /// </summary>
        [JsonPropertyName("azp")]
        public string? Azp { get; set; }
        /// <summary>
        /// The user email address. This may not be unique and is not suitable for use as a primary key. Provided only if your scope included the string "email".
        /// </summary>
        [JsonPropertyName("email")]
        public string? Email { get; set; }
        /// <summary>
        /// The time the ID token was issued, represented in Unix time (integer seconds).
        /// </summary>
        [JsonPropertyName("iat")]
        public string? Iat { get; set; }
        /// <summary>
        /// The time the ID token expires, represented in Unix time (integer seconds).
        /// </summary>
        [JsonPropertyName("exp")]
        public string? Exp { get; set; }
        /// <summary>
        /// The user full name, in a displayable form. Might be provided when:
        /// The request scope included the string "profile"
        /// The ID token is returned from a token refresh
        /// When name claims are present, you can use them to update your app user records. Note that this claim is never guaranteed to be present.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        /// <summary>
        /// The URL of the user profile picture. Might be provided when:
        /// The request scope included the string "profile"
        /// The ID token is returned from a token refresh
        /// When picture claims are present, you can use them to update your app user records. Note that this claim is never guaranteed to be present.
        /// </summary>
        [JsonPropertyName("picture")]
        public string? Picture { get; set; }
        /// <summary>
        /// given_name
        /// </summary>
        [JsonPropertyName("given_name")]
        public string? GivenName { get; set; }
        /// <summary>
        /// family_name
        /// </summary>
        [JsonPropertyName("family_name")]
        public string? FamilyName { get; set; }
        /// <summary>
        /// locale
        /// </summary>
        [JsonPropertyName("locale")]
        public string? Locale { get; set; }
        /// <summary>
        /// alg
        /// </summary>
        [JsonPropertyName("alg")]
        public string? Alg { get; set; }
        /// <summary>
        /// kid
        /// </summary>
        [JsonPropertyName("kid")]
        public string? Kid { get; set; }
        /// <summary>
        /// error_description
        /// </summary>
        [JsonPropertyName("error_description")]
        public string? ErrorDescription { get; set; }
    }
}