
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Models;

namespace LeadsHub.Core.Interfaces.IRepository
{
    public interface ISalesPipelineRepository
    {
        Task<SimpleResponse<SalesPipeline>> FetchPipelineByIdAsync(long pipelineId);
    }
}
