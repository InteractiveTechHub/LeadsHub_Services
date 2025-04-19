
using AdaptiveKitCore.Enums;
using AdaptiveKitCore.Requests;
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Interfaces.IBac;
using LeadsHub.Core.Interfaces.IRepository;
using LeadsHub.Core.Models;
using LeadsHub.Core.Responses;

namespace LeadsHub.Core.Bac
{
    public sealed class SalesPipelineBac : ISalesPipelineBac
    {
        private readonly ISalesPipelineRepository _salesPipelineRepository;
        public SalesPipelineBac(ISalesPipelineRepository salesPipelineRepository)
        {
            _salesPipelineRepository = salesPipelineRepository; 
        }

        public async Task<SimpleResponse<LeadStage>> CreateLeadStageAsync(Lead lead)
        {
            SimpleResponse<LeadStage> response = new();

            FilterRequest filterRequest = new();
            filterRequest.AddFilter(nameof(SalesPipeline.CompanyId), FilterOperatorEnum.Equals, lead.CompanyId);

            SalesPipelineResponse pipeResponse = await FetchPipelinesByRequestAsync(filterRequest);

            long pipelineId = pipeResponse.ResponseData.Select(p => p.Id).FirstOrDefault();

            PipelineStageResponse stageResponse = await _salesPipelineRepository.FetchPipelineStageByPipeIdAsync(pipelineId);

            PipelineStage? stage = stageResponse.ResponseData.OrderBy(r => r.Position).FirstOrDefault();
            if (stage is null)
            {
                // TODO: log error
                response.AddErrorMessage("Not possible to found the the pipeline stage", "PipelineStageNotFound");
                
                return response;
            }

            LeadStage leadStage = new();
            leadStage.LeadId = lead.Id;
            leadStage.PipelineStageId = stage.Id;

            response = await _salesPipelineRepository.CreateLeadStageAsync(leadStage);
            if (response.HasAnyErrorMessage)
            {
                return response;
            }

            return response;
        }

        public async Task<SimpleResponse<SalesPipeline>> CreatePipelineAsync(SalesPipeline salesPipeline)
        {
            if (string.IsNullOrWhiteSpace(salesPipeline.Name))
            {
                salesPipeline.Name = "Funil Principal";
                salesPipeline.Position = 0;
            }               

            ICollection<PipelineStage> stageList = BuildDefaultStage();

            salesPipeline.Stages.AddRange(stageList);

            return await _salesPipelineRepository.CreatePipelineAsync(salesPipeline);
        }  

        public async Task<SimpleResponse<SalesPipeline>> FetchPipelineByIdAsync(long pipelineId)
        {
            return await _salesPipelineRepository.FetchPipelineByIdAsync(pipelineId);
        }

        public async Task<SalesPipelineResponse> FetchPipelinesByRequestAsync(FilterRequest filterRequest)
        {
            return await _salesPipelineRepository.FetchPipelinesByRequestAsync(filterRequest);
        }

        public async Task<ModelResponse> UpdatePipelinesAsync(List<SalesPipeline> salesPipelineList)
        {
            return await _salesPipelineRepository.UpdatePipelinesAsync(salesPipelineList);
        }

        public async Task<ModelResponse> UpdatePipelineStageAsync(PipelineStage stage)
        {
            return await _salesPipelineRepository.UpdatePipelineStageAsync(stage);
        }

        public async Task<ModelResponse> UpdateLeadStageAsync(List<LeadStage> leadStageList, long? stageId)
        {
            // TODO: Fetch LeadStage that is not in the same PipelineStageId
            if (stageId is not null)
            {
                string leadStageIds = string.Join(", ", leadStageList.Select(s => s.Id).ToList());

                FilterRequest filterRequest = new();
                filterRequest.AddFilter(nameof(LeadStage.Id), FilterOperatorEnum.In, leadStageIds);
                filterRequest.AddFilter(nameof(LeadStage.PipelineStageId), FilterOperatorEnum.NotEquals, stageId);

                var response = await FetchLeadStageByRequest(filterRequest);

                LeadStage? previousLeadStage = response.ResponseData.FirstOrDefault();
                if (previousLeadStage is not null)
                {
                    leadStageList.ForEach(stage =>
                    {
                        if (stage.Id == previousLeadStage.Id)
                        {
                            stage.MovedAt = DateTimeOffset.UtcNow;
                        }
                    });
                }
            }   

            return await _salesPipelineRepository.UpdateLeadStageAsync(leadStageList);
        }

        public async Task<LeadStageResponse> FetchLeadStageByRequest(FilterRequest filterRequest)
        {
            return await _salesPipelineRepository.FetchLeadStageByRequest(filterRequest);
        }

        private ICollection<PipelineStage> BuildDefaultStage()
        {
            ICollection<PipelineStage> stageList =
            [
                new()
                {
                    Title = "Novo Lead",
                    Position = 0,
                },
                new() {
                    Title = "Em negociação",
                    Position = 1,
                },
                new()
                {
                    Title = "Em agendamento",
                    Position = 2
                },
                new()
                {
                    Title = "Finalizado",
                    Position = 3
                }
            ];

            return stageList;
        }
    }
}
