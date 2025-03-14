namespace LeadsHub.Core.Models
{
    public sealed class WhatsAppConfig
    {
        public int Id { get; set; }

        public string AccessToken { get; set; } = string.Empty;

        public string BusinessAccountId { get; set; } = string.Empty;

        public string PhoneNumberId { get; set; } = string.Empty;
    }
}
