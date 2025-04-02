using LeadsHub.Core.Enum;

namespace LeadsHub.Core.Models
{
    public class Integration
    {
        public long Id { get; set; }

        public long CompanyId { get; set; }

        public string Name { get; set; } = string.Empty;

        public IntegrationType Type { get; set; }

        public long WhatsAppConfigId { get; set; }

        public WhatsAppConfig? WhatsAppConfig { get; set; }
    }
}
