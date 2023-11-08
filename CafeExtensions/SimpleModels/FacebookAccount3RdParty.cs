using System.Text.Json;
using System.Text.Json.Serialization;

namespace CafeExtensions.SimpleModels
{
    /// <summary>
    /// Login with facebook (Meta)
    /// </summary>
    public class FacebookAccount3RdParty : Account3rdParty
    {
        /// <summary>
        /// adress check token
        /// </summary>
        protected override string ValidationUrl { get { return "https://graph.facebook.com/me?fields=email,name,id&access_token={0}"; } }
        /// <summary>
        /// Validate account 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public override async Task<AccoResponse?> ValidateAccount(string token)
        {
            var response = await GetResultAsync(token);
            if (response == null) return null;
            if (response.Message == null) return null;
            var fbApiTokenInfo = JsonSerializer.Deserialize<FacebookApiTokenInfo>(response.Message);
            if (fbApiTokenInfo == null) return null;
            var accoResponse = new AccoResponse();
            if (response.Code != System.Net.HttpStatusCode.OK)
                accoResponse.Error = new AccoResponseError { Message = fbApiTokenInfo.Error?.Message, Code = fbApiTokenInfo.Error?.Code };
            else
                accoResponse.Social = new AccountSocialInfo { Email = fbApiTokenInfo.Email!, Name = fbApiTokenInfo.Name!, ExternalId = fbApiTokenInfo.Id!, Provider = ProviderName.Facebook };

            return accoResponse;
        }
    }
    /// <summary>
    /// Answers facebook
    /// </summary>
    public class FacebookApiTokenInfo
    {
        /// <summary>
        /// Unique user id
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        /// <summary>
        /// The user full name, in a displayable form. Might be provided when:
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        /// <summary>
        /// The user email address. This may not be unique and is not suitable for use as a primary key. Provided only if your scope included the string "email".
        /// </summary>
        [JsonPropertyName("email")]
        public string? Email { get; set; }
        /// <summary>
        /// Error text
        /// </summary>
        public FacebookApiTokenError? Error { get; set; }
    }
    /// <summary>
    /// FacebookApiTokenError
    /// </summary>
    public class FacebookApiTokenError
    {
        /// <summary>
        /// message
        /// </summary>
        [JsonPropertyName("message")]
        public string? Message { get; set; }
        /// <summary>
        /// type
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        /// <summary>
        /// code
        /// </summary>
        [JsonPropertyName("code")]
        public string? Code { get; set; }
    }
}