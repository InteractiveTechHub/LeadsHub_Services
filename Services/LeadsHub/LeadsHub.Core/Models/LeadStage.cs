namespace LeadsHub.Core.Models
{
    public sealed class LeadStage : BaseModel
    {
        public long LeadId { get; set; }

        public long PipelineStageId { get; set; } = default!;

        public int Position { get; set; } = 0;

        public DateTimeOffset MovedAt { get; set; }

        public LeadCard LeadCard { get; set; } = new();
    }
}
