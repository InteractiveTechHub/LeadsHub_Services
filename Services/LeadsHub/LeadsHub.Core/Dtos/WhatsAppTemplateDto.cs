
using LeadsHub.Core.Enum;
using LeadsHub.Core.Models;

namespace LeadsHub.Core.Dtos
{
    public class WhatsAppTemplateDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string TemplateBodyMirror { get; set; } = string.Empty;

        public TemplateType TemplateType { get; set; }
    }
}
