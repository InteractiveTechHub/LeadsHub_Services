
using AdaptiveKitCore.Requests;
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Models;
using LeadsHub.Core.Responses;

namespace LeadsHub.Core.Interfaces.IBac
{
    public interface ISalesPipelineBac
    {
        Task<SimpleResponse<LeadStage>> CreateLeadStageAsync(Lead lead);

        Task<SimpleResponse<SalesPipeline>> CreatePipelineAsync(SalesPipeline salesPipeline);

        Task<LeadStageResponse> FetchLeadStageByRequest(FilterRequest filterRequest);

        Task<SimpleResponse<SalesPipeline>> FetchPipelineByIdAsync(long pipelineId);

        Task<SalesPipelineResponse> FetchPipelinesByRequestAsync(FilterRequest filterRequest);

        Task<ModelResponse> UpdatePipelinesAsync(List<SalesPipeline> salesPipelineList);

        Task<ModelResponse> UpdatePipelineStageAsync(PipelineStage stage);

        Task<ModelResponse> UpdateLeadStageAsync(List<LeadStage> leadStageList, long? stageId);
    }
}
