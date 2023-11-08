namespace CafeExtensions.SimpleModels
{
    public class AccountSocialVkBot
    {
        /// <summary>
        /// Token social network
        /// </summary>
        public string? ChatId { get; set; }
        /// <summary>
        /// OrganizationId
        /// </summary>
        public Guid? OrganizationId { get; set; }

        public bool Validate()
        {
            if (ChatId == null)
                return false;
            if (OrganizationId == null)
                return false;
            if (ChatId.Length <= 0)
                return false;
            return true;
        }

        public AccountSocialInfo GetAccountSocialInfo()
        {
            return new AccountSocialInfo()
            {
                Email = null,
                Phone = String.Empty,
                ExternalId = ChatId,
                Id = ChatId,
                Locale = "Ru",
                Name = null,
                OrganizationId = OrganizationId,
                Provider = ProviderName.VkBot
            };
        }
    }
}
