using LeadsHub.Core.Enum;

namespace LeadsHub.Core.Models
{
    public class Integration : BaseModel
    {
        public long CompanyId { get; set; }

        public long? WhatsAppConfigId { get; set; }

        public Company Company { get; set; } = new();

        public WhatsAppConfig? WhatsAppConfig { get; set; }
    }
}
