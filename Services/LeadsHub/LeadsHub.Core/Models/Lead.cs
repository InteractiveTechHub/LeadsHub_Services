
using LeadsHub.Core.Enum;

namespace LeadsHub.Core.Models
{
    public sealed class Lead : BaseModel
    {
        public long CompanyId { get; set; }

        public long? ConsultantId { get; set; }

        public long ContactId { get; set; }

        public long? CampaignId { get; set; }

        public int Channel { get; set; }

        /// <summary>
        /// Postgres Default UUID 
        /// </summary>
        public Guid Identifier { get; set; }

        public long IntegrationId { get; set; }

        public LeadPhase Phase { get; set; } = LeadPhase.New;

        public int Status { get; set; } = 1; // This will be enumerator

        public Consultant? Consultant { get; set; }

        public Contact Contact { get; set; } = new();

        public Integration Integration { get; set; } = new();

        public List<Timeline> Timelines { get; set; } = new();
    }
}
