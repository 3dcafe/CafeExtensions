using System.Text.Json.Serialization;

namespace CafeExtensions.SimpleModels
{
    /// <summary>
    /// Social model login
    /// </summary>
    public sealed class AccountSocialInfo
    {
        /// <summary>
        /// Provider
        /// </summary>
        [JsonPropertyName("provider")]
        public ProviderName Provider { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [JsonPropertyName("email")]
        public string? Email { get; set; }
        /// <summary>
        /// ExternalId
        /// </summary>
        [JsonPropertyName("external_id")]
        public string? ExternalId { get; set; }
        /// <summary>
        /// Locale
        /// </summary>
        [JsonPropertyName("locale")]
        public string? Locale { get; set; }
        /// <summary>
        /// Phone user
        /// </summary>
        public string? Phone { get; set; }
        /// <summary>
        /// Organization link by Id
        /// </summary>
        [JsonPropertyName("organizationId")]
        public Guid? OrganizationId { get; set; }
        /// <summary>
        /// To string object
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Provider - {Provider} Id - {Id} Name - {Name} Email - {Email} ExternalId - {ExternalId}";
        }
    }
}
