namespace LeadsHub.Core.Models
{
    public sealed class WhatsAppConfig : BaseModel
    {
        public long CompanyId { get; set; }

        public string AccessToken { get; set; } = string.Empty;

        public string BusinessAccountId { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string PhoneNumberId { get; set; } = string.Empty;

        public string WebHookSecret { get; set; } = string.Empty;

        public bool Enabled { get; set; }

        public Company Company { get; set; } = new();

        public List<WhatsAppTemplate> WhatsAppTemplates { get; set; } = [];
    }
}
