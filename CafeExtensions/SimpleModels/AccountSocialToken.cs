namespace CafeExtensions.SimpleModels
{
    public class AccountSocialToken
    {
        /// <summary>
        /// Token social network
        /// </summary>
        public string? Token { get; set; }
        /// <summary>
        /// OrganizationId
        /// </summary>
        public Guid? OrganizationId { get; set; }
    }
}
