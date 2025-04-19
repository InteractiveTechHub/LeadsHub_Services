
namespace LeadsHub.Core.Models
{
    public sealed class PipelineStage : BaseModel
    {
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Position/order of the stage (columns) displayed
        /// </summary>
        public int Position { get; set; }

        public long SalesPipelineId { get; set; }

        public ICollection<LeadStage> Leads { get; set; } = [];
    }
}
