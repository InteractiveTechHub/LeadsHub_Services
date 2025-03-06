
namespace WhatsApp.Core.Models
{
    public class Integration
    {
        public long Id { get; set; }

        public long CompanyId { get; set; }

        public long WhatsAppConfigId { get; set; }

        public WhatsAppConfig? WhatsappConfig { get; set; }
    }
}
