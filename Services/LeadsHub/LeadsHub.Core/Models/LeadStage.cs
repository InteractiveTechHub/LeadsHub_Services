namespace LeadsHub.Core.Models
{
    public sealed class LeadStage : BaseModel
    {
        public int LeadId { get; set; }

        public long PipelineStageId { get; set; } = default!;

        public int Position { get; set; }

        public LeadCard LeadCard { get; set; } = new();
    }
}
