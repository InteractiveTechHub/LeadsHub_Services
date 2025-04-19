
using AdaptiveKitCore.Requests;
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Models;
using LeadsHub.Core.Responses;

namespace LeadsHub.Core.Interfaces.IRepository
{
    public interface ISalesPipelineRepository
    {
        Task<SimpleResponse<LeadStage>> CreateLeadStageAsync(LeadStage leadStage);

        Task<SimpleResponse<SalesPipeline>> CreatePipelineAsync(SalesPipeline salesPipeline);

        Task<LeadStageResponse> FetchLeadStageByRequest(FilterRequest filterRequest);

        Task<SimpleResponse<SalesPipeline>> FetchPipelineByIdAsync(long pipelineId);

        Task<SalesPipelineResponse> FetchPipelinesByRequestAsync(FilterRequest filterRequest);

        Task<PipelineStageResponse> FetchPipelineStageByPipeIdAsync(long salesPipelineId);

        Task<ModelResponse> UpdatePipelinesAsync(List<SalesPipeline> salesPipelineList);

        Task<ModelResponse> UpdatePipelineStageAsync(PipelineStage stage);

        Task<ModelResponse> UpdateLeadStageAsync(IEnumerable<LeadStage> leadStageList);
    }
}
