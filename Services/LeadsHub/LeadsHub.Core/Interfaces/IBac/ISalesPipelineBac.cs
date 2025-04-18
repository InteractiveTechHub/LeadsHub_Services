
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Models;

namespace LeadsHub.Core.Interfaces.IBac
{
    public interface ISalesPipelineBac
    {
        Task<SimpleResponse<SalesPipeline>> FetchPipelineByIdAsync(long pipelineId);
    }
}
