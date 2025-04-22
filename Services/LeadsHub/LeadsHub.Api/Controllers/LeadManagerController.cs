
using AdaptiveKitCore.Requests;
using LeadsHub.Core.Dtos;
using LeadsHub.Core.Interfaces.IBac;
using LeadsHub.Core.Models;
using LeadsHub.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LeadsHub.Api.Controllers
{    public class LeadManagerController : BaseController
    {
        private readonly ILeadManagerBac _leadManagerBac;
        public LeadManagerController(ILeadManagerBac leadManagerBac)
        {
            _leadManagerBac = leadManagerBac;
        }

        [HttpPost("leadcards")]
        public async Task<IActionResult> FetchLeadCardsByRequestAsync(FilterRequest filterRequest)
        {
            filterRequest.AddSortExpressionDescending("CreatedAt");

           LeadCardResponse response = await _leadManagerBac.FetchCardsByRequestAsync(filterRequest);
            if (response.HasAnyErrorMessage)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("templates")]
        public async Task<IActionResult> FetchTemplatesAsync(long leadId)
        {
            BaseResponse<TemplatesPerType> response = await _leadManagerBac.FetchTemplatesAsync(leadId);
            if (response.HasAnyErrorMessage)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("close-lead")]
        public async Task<IActionResult> CloseLeadAsync(LeadCard leadCard)
        {
            var response = await _leadManagerBac.CloseLeadAsync(leadCard);
            if (response.HasAnyErrorMessage) 
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
