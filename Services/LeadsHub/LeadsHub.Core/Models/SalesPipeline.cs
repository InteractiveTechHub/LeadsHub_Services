
namespace LeadsHub.Core.Models
{
    public sealed class SalesPipeline : BaseModel
    {
        public long CompanyId { get; set; }

        public long ConsultantId { get; set; }

        public string Name { get; set; } = string.Empty;

        public ICollection<PipelineStage> Stages { get; set; } = [];
    }
}
