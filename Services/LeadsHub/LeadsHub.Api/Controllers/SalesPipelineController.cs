using AdaptiveKitCore.Enums;
using AdaptiveKitCore.Requests;
using InteractiveLeads.Core.Enums;
using LeadsHub.Api.Services;
using LeadsHub.Core.Identity;
using LeadsHub.Core.Interfaces.IBac;
using LeadsHub.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeadsHub.Api.Controllers
{
    public class SalesPipelineController : BaseController
    {
        private readonly ISalesPipelineBac _salesPipelineBac;
        private readonly IUserContextService _userContextService;

        public SalesPipelineController(ISalesPipelineBac salesPipelineBac, IUserContextService userContextService)
        {
            _salesPipelineBac = salesPipelineBac;
            _userContextService = userContextService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePipelineAsync([FromBody] SalesPipeline salesPipeline)
        {
            var response = await _salesPipelineBac.CreatePipelineAsync(salesPipeline);
            if (response.HasAnyErrorMessage)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("fetch-pipelines")]
        public async Task<IActionResult> FetchPipelinesByRequestAsync([FromBody] FilterRequest filterRequest)
        {
            if (User.IsInRole(RolesEnum.Consultant.Name))
            {
                UserContext userContext = await _userContextService.GetUserContextAsync();
                filterRequest.AddFilterDescriptors(userContext.FilterRequest.FilterDescriptors);
            }

            var response = await _salesPipelineBac.FetchPipelinesByRequestAsync(filterRequest);
            if (response.HasAnyErrorMessage)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> FetchSalesPipelineByIdAsync(long id)
        {
            var response = await _salesPipelineBac.FetchPipelineByIdAsync(id);
            if (response.HasAnyErrorMessage)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePipelinesAsync([FromBody] List<SalesPipeline> salesPipelineList)
        {
            var response = await _salesPipelineBac.UpdatePipelinesAsync(salesPipelineList);
            if (response.HasAnyErrorMessage)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut("stage")]
        public async Task<IActionResult> UpdatePipelineStageAsync([FromBody] PipelineStage stage)
        {
            var response = await _salesPipelineBac.UpdatePipelineStageAsync(stage);
            if (response.HasAnyErrorMessage)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut("leadStage")]
        public async Task<IActionResult> UpdateLeadStageAsync([FromBody] List<LeadStage> leadStage, [FromQuery] long? stageId)
        {
            var response = await _salesPipelineBac.UpdateLeadStageAsync(leadStage, stageId);
            if (response.HasAnyErrorMessage)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
