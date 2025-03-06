
namespace LeadsHub.Core.Models
{
    public sealed class Lead : BaseModel
    {
        public long CompanyId { get; set; }

        public long? ConsultantId { get; set; }

        public long ContactId { get; set; }

        public long? CampaignId { get; set; }

        public int Channel { get; set; }

        public long IntegrationId { get; set; }

        public int Status { get; set; } = 1; // This will be enumerator

        public Consultant? Consultant { get; set; }

        public Contact Contact { get; set; } = new();
    }
}
