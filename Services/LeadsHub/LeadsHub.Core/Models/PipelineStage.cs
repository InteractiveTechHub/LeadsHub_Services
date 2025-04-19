
namespace LeadsHub.Core.Models
{
    public sealed class PipelineStage : BaseModel
    {
        public string Title { get; set; } = string.Empty;

        public int StageOrder { get; set; }

        public long SalesPipelineId { get; set; }

        public ICollection<LeadStage> Leads { get; set; } = [];
    }
}
