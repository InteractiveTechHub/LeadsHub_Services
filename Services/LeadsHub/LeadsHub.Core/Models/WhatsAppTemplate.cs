
using LeadsHub.Core.Enum;

namespace LeadsHub.Core.Models
{
    public class WhatsAppTemplate : BaseModel
    {
        public long WhatsAppConfigId { get; set; }

        /// <summary>
        /// Name of the template according to whatsapp manager
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Body of the template.
        /// </summary>
        public string TemplateBodyMirror { get; set; } = string.Empty;

        /// <summary>
        /// Type of the template according CRM Rules
        /// </summary>
        public TemplateType Type { get; set; }

        /// <summary>
        /// Variables of the template, according to CRM Rules
        /// </summary>
        public string Variables { get; set; } = string.Empty;

        /// <summary>
        /// Language of the template, set in whatsapp manager (meta)
        /// </summary>
        public string Language { get; set; } = string.Empty;

        /// <summary>
        /// Category of the template, set in whatsapp manager (meta)
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Status of the template, set in whatsapp manager (meta)
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Enable or disable the template in CRM (not in the meta)
        /// </summary>
        public bool Enabled { get; set; }

        public WhatsAppConfig WhatsAppConfig { get; set; } = new();
    }
}
