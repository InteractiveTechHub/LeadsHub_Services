
namespace LeadsHub.Core.Models
{
    public sealed class SalesPipeline : BaseModel
    {
        public long CompanyId { get; set; }

        public long? ConsultantId { get; set; }

        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Position/order of pipeline displayed on the Menu.
        /// </summary>
        public int Position { get; set; }

        public List<PipelineStage> Stages { get; set; } = [];

        public Company Company { get; set; } = new();
        public Consultant? Consultant { get; set; }
    }
}
