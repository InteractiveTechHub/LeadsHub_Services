
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Interfaces.IBac;
using LeadsHub.Core.Interfaces.IRepository;
using LeadsHub.Core.Models;

namespace LeadsHub.Core.Bac
{
    public sealed class SalesPipelineBac : ISalesPipelineBac
    {
        private readonly ISalesPipelineRepository _salesPipelineRepository;
        public SalesPipelineBac(ISalesPipelineRepository salesPipelineRepository)
        {
            _salesPipelineRepository = salesPipelineRepository; 
        }

        public async Task<SimpleResponse<SalesPipeline>> FetchPipelineByIdAsync(long pipelineId)
        {
            return await _salesPipelineRepository.FetchPipelineByIdAsync(pipelineId);
        }
    }
}
