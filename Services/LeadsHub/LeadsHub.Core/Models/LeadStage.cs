namespace LeadsHub.Core.Models
{
    public sealed class LeadStage : BaseModel
    {
        public long LeadId { get; set; }

        public long PipelineStageId { get; set; } = default!;

        /// <summary>
        /// Position/Order displayed within the stage (column)
        /// </summary>
        public int Position { get; set; } = 0;

        public DateTimeOffset MovedAt { get; set; }

        public LeadCard LeadCard { get; set; } = new();

        public Lead Lead { get; set; } = new();
        public PipelineStage PipelineStage { get; set; } = new();
    }
}
