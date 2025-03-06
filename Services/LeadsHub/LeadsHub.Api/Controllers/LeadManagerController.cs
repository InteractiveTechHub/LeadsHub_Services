
using AdaptiveKitCore.Requests;
using LeadsHub.Core.Interfaces.IBac;
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
    }
}
