namespace CafeExtensions.SimpleModels
{
    /// <summary>
    /// Base object token social network
    /// </summary>
    public class AccountSocialTelegram
    {
        /// <summary>
        /// Token social network
        /// </summary>
        public string? ChatId { get; set; }
        /// <summary>
        /// Phone user for find
        /// </summary>
        public string? Phone { get; set; }
        /// <summary>
        /// OrganizationId
        /// </summary>
        public Guid? OrganizationId { get; set; }

        public bool Validate()
        {
            if (ChatId == null)
                return false;
            if (Phone == null)
                return false;
            if (OrganizationId == null)
                return false;
            if (ChatId.Length <= 0 || Phone.Length <= 0)
                return false;
            return true;
        }

        public AccountSocialInfo GetAccountSocialInfo()
        {
            return new AccountSocialInfo()
            {
                Email = null,
                Phone = Phone,
                ExternalId = ChatId,
                Id = ChatId,
                Locale = "Ru",
                Name = null,
                OrganizationId = OrganizationId,
                Provider = ProviderName.Telegram
            };
        }
    }
}
