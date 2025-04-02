
using LeadsHub.Core.Enum;

namespace LeadsHub.Core.Dtos
{
    public class TemplatesPerType
    {
        public TemplateType TemplateType { get; set; }
        public List<WhatsAppTemplateDto> Templates { get; set; } = [];

        public bool IsAppointmentTemplate => TemplateType.Equals(TemplateType.Appointment);

        public bool IsCustomerFeedBackTemplate => TemplateType.Equals(TemplateType.CustomerFeedback);

        public bool IsFollowUpTemplate => TemplateType.Equals(TemplateType.FollowUp);

        public bool IsWelcomeMessage => TemplateType.Equals(TemplateType.WelcomeMessage);
    }
}
